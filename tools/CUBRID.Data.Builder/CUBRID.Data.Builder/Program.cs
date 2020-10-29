using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LibGit2Sharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CUBRID.Data.Builder
{
    internal static class Program
    {
        private const string cubridAdonet = "https://github.com/CUBRID/cubrid-adonet.git";
        private const string cubridBranch = "release/10.2";

        private const string indent = "    ";

        private static string _cloneDir;
        private static string _projectDir;

        private static void Main(string[] args)
        {
            var directory = Path.GetFullPath("temp");

            if (Directory.Exists(directory))
                DeleteDirectory(directory);

            Directory.CreateDirectory(directory);
            Environment.CurrentDirectory = directory;

            _cloneDir = Path.GetFullPath("clone");
            _projectDir = Path.GetFullPath("../../../../../../../src/CUBRID.Data");

            CloneRepository();
            CleanProject();
            Generate();
        }

        private static void DeleteDirectory(string directory)
        {
            foreach (var subdirectory in Directory.EnumerateDirectories(directory))
            {
                DeleteDirectory(subdirectory);
            }

            foreach (var fileName in Directory.EnumerateFiles(directory))
            {
                var fileInfo = new FileInfo(fileName)
                {
                    Attributes = FileAttributes.Normal
                };

                fileInfo.Delete();
            }

            Directory.Delete(directory);
        }

        private static void CloneRepository()
        {
            var cloneOptions = new CloneOptions
            {
                BranchName = cubridBranch
            };

            cloneOptions.OnProgress += p =>
            {
                Console.WriteLine(p);
                return true;
            };

            Repository.Clone(cubridAdonet, _cloneDir, cloneOptions);
        }

        private static void CleanProject()
        {
            var excludeDirs = new[] { ".idea" };
            var excludeFiles = new[] { "CUBRID.Data.csproj" };

            foreach (var file in Directory.GetFiles(_projectDir))
            {
                if (excludeFiles.Contains(Path.GetFileName(file)))
                    continue;

                File.Delete(file);
            }

            foreach (var directory in Directory.GetDirectories(_projectDir))
            {
                if (excludeDirs.Contains(Path.GetFileName(directory)))
                    continue;

                Directory.Delete(directory, true);
            }
        }

        private static void Generate()
        {
            var includeExts = new[] { ".cs", ".resx" };

            var excludeFiles = new[]
            {
                @"DataType\CUBRIDArray.cs",
                @"DataType\CUBRIDLobHandle.cs",
                @"Properties\AssemblyInfo.cs"
            };

            var srcDir = Path.Combine(_cloneDir, "Code", "Src");

            foreach (var file in Directory.GetFiles(srcDir, "*.*", SearchOption.AllDirectories))
            {
                string extension = Path.GetExtension(file).ToLower();

                if (!includeExts.Contains(extension))
                    continue;

                var relativePath = Path.GetRelativePath(srcDir, file);

                if (excludeFiles.Contains(relativePath, StringComparer.OrdinalIgnoreCase))
                    continue;

                var destination = Path.Combine(_projectDir, relativePath);
                var destinationDir = Path.GetDirectoryName(destination);

                if (!Directory.Exists(destinationDir))
                    Directory.CreateDirectory(destinationDir);

                // patch

                if (extension != ".cs")
                {
                    File.Copy(file, destination, true);
                    continue;
                }

                string code = File.ReadAllText(file);

                code = code.Replace("CUBRID.Data.CUBRIDClient", "CUBRID.Data");

                switch (relativePath)
                {
                    case "Util.cs":
                        code = Regex.Replace(code, @"ResourceManager rm =[^;]+;", string.Empty);
                        code = code.Replace("return rm.GetString", "return Resource.ResourceManager.GetString");
                        break;

                    case "CUBRIDCciInterface.cs":
                        code = PatchCciInterface(code);
                        break;
                }

                File.WriteAllText(destination, code);
            }
        }

        private static string PatchCciInterface(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var classNode = tree.GetRoot().DescendantNodes().First(n => n is ClassDeclarationSyntax);

            InteropMethod[] methods = classNode.ChildNodes()
                .Where(n =>
                    n is MethodDeclarationSyntax m &&
                    m.AttributeLists.Count == 1 &&
                    m.AttributeLists[0].Attributes.Count == 1 &&
                    m.AttributeLists[0].Attributes[0].Name.ToString() == "DllImport")
                .Cast<MethodDeclarationSyntax>()
                .Select(m => new InteropMethod
                {
                    Attribute = m.AttributeLists[0].NormalizeWhitespace().ToFullString(),
                    Modifier = m.Modifiers[0].NormalizeWhitespace().ToFullString(),
                    ReturnType = m.ReturnType.NormalizeWhitespace().ToFullString(),
                    Name = m.Identifier.Text,
                    Parameter = m.ParameterList.NormalizeWhitespace().ToFullString()[1..^1],
                    Parameters = m.ParameterList.Parameters
                        .Select(p => new ParameterInfo
                        {
                            Modifier = p.Modifiers.ToFullString().Trim(),
                            Name = p.Identifier.Text,
                            Type = p.Type!.NormalizeWhitespace().ToFullString()
                        })
                        .ToArray(),
                    Span = m.Span
                })
                .ToArray();

            var runtimeDir = Path.Combine(_projectDir, "Runtime");
            Directory.CreateDirectory(runtimeDir);

            GenerateInterop(runtimeDir, "Win64", "runtimes/win-x64/native/cascci64.dll", methods);
            GenerateInterop(runtimeDir, "Win86", "runtimes/win-x86/native/cascci86.dll", methods);
            GenerateInterop(runtimeDir, "Linux", "runtimes/linux/native/libcascci.so", methods);

            GenerateRuntimeInterface(runtimeDir, methods);

            GenerateRuntime(runtimeDir, "Win64", methods);
            GenerateRuntime(runtimeDir, "Win86", methods);
            GenerateRuntime(runtimeDir, "Linux", methods);

            var buffer = new StringBuilder(code);
            var dllNameMatch = Regex.Match(code, @"\s+const\s+string\s+dll_name\s+[^;]+;");
            var initializer = ResourceUtility.GetResource("CciInterfaceInitialize.txt");

            buffer.Remove(dllNameMatch.Index, dllNameMatch.Length);
            buffer.Insert(dllNameMatch.Index, initializer);

            int offset = initializer.Length - dllNameMatch.Length;

            foreach (var method in methods)
            {
                var proxy = new StringBuilder();

                proxy.AppendLine($"public static {method.ReturnType} {method.Name}({method.Parameter})");

                proxy.Append(indent).Append(indent).Append(indent)
                    .Append($"=> _runtime.{method.Name}(");

                WriteParameterPassing(proxy, method);
                proxy.Append(");");

                buffer.Remove(offset + method.Span.Start, method.Span.Length);
                buffer.Insert(offset + method.Span.Start, proxy);

                offset += proxy.Length - method.Span.Length;
            }

            return buffer.ToString();
        }

        private static void GenerateRuntimeInterface(string runtimeDir, IEnumerable<InteropMethod> methods)
        {
            var path = Path.Combine(runtimeDir, "IRuntime.cs");
            var body = new StringBuilder();

            foreach (var method in methods)
            {
                if (body.Length > 0)
                {
                    body.AppendLine();
                    body.AppendLine();
                }

                body.Append(indent).Append(indent)
                    .AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");

                body.Append(indent).Append(indent)
                    .Append($"{method.ReturnType} {method.Name}({method.Parameter});");
            }

            var code = ResourceUtility.GetResource("IRuntime.txt")
                .Replace(":BODY:", body.ToString());

            File.WriteAllText(path, code);
        }

        private static void GenerateRuntime(string runtimeDir, string runtime, IEnumerable<InteropMethod> methods)
        {
            var path = Path.Combine(runtimeDir, $"{runtime}Runtime.cs");
            var body = new StringBuilder();

            foreach (var method in methods)
            {
                if (body.Length > 0)
                {
                    body.AppendLine();
                    body.AppendLine();
                }

                body.Append(indent).Append(indent)
                    .AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");

                body.Append(indent).Append(indent)
                    .AppendLine($"public {method.ReturnType} {method.Name}({method.Parameter})");

                body.Append(indent).Append(indent).Append(indent)
                    .Append($"=> {runtime}Interop.{method.Name}(");

                WriteParameterPassing(body, method);

                body.Append(");");
            }

            var code = ResourceUtility.GetResource("Runtime.txt")
                .Replace(":RUNTIME:", runtime)
                .Replace(":BODY:", body.ToString());

            File.WriteAllText(path, code);
        }

        private static void GenerateInterop(string runtimeDir, string runtime, string dll, IEnumerable<InteropMethod> methods)
        {
            var path = Path.Combine(runtimeDir, $"{runtime}Interop.cs");
            var body = new StringBuilder();

            foreach (var method in methods)
            {
                if (body.Length > 0)
                {
                    body.AppendLine();
                    body.AppendLine();
                }

                body.Append(indent).Append(indent)
                    .AppendLine(method.Attribute);

                body.Append(indent).Append(indent)
                    .Append($"{method.Modifier} static extern {method.ReturnType} {method.Name}({method.Parameter});");
            }

            var code = ResourceUtility.GetResource("Interop.txt")
                .Replace(":RUNTIME:", runtime)
                .Replace(":DLL:", dll)
                .Replace(":BODY:", body.ToString());

            File.WriteAllText(path, code);
        }

        private static void WriteParameterPassing(StringBuilder body, InteropMethod method)
        {
            for (int i = 0; i < method.Parameters.Length; i++)
            {
                if (i > 0)
                    body.Append(", ");

                if (method.Parameters[i].Modifier == "ref")
                    body.Append("ref ");

                body.Append(method.Parameters[i].Name);
            }
        }

        private class InteropMethod
        {
            public string Attribute { get; set; }

            public string Modifier { get; set; }

            public string Name { get; set; }

            public string ReturnType { get; set; }

            public ParameterInfo[] Parameters { get; set; }

            public string Parameter { get; set; }

            public TextSpan Span { get; set; }
        }

        private class ParameterInfo
        {
            public string Modifier { get; set; }

            public string Type { get; set; }

            public string Name { get; set; }
        }
    }
}
