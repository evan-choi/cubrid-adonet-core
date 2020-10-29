using System.IO;
using System.Reflection;

namespace CUBRID.Data.Builder
{
    public static class ResourceUtility
    {
        private static readonly Assembly _assembly = typeof(ResourceUtility).Assembly;

        public static string GetResource(string name)
        {
            var stream = _assembly.GetManifestResourceStream($"CUBRID.Data.Builder.Resources.{name}");
            var reader = new StreamReader(stream!);
            return reader.ReadToEnd();
        }
    }
}
