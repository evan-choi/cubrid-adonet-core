Function Resolve-Path-Safe {
    Param (
        [Parameter(Mandatory=$true)][string] $Value
    )

    return $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($Value)
}

Function Remove-Directory-Safe {
    Param (
        [Parameter(Mandatory=$true)][string] $Value
    )

    if (Test-Path $Value) {
        Remove-Item -Path $Value -Recurse -Force -Confirm:$false -ErrorAction Ignore
    }
}

Function DotNet-Pack {
    Param (
        [Parameter(Mandatory = $true)][string] $ProjectName
    )

    Write-Host "[.NET] $($ProjectName) Pack" -ForegroundColor Cyan

    Remove-Directory-Safe "$ProjectName/bin"
    Remove-Directory-Safe "$ProjectName/obj"

    dotnet pack $ProjectName `
        --nologo `
        -v=q `
        -c Release `
        -o "./Build" `
        -p:Packaging=true
}

DotNet-Pack "./src/CUBRID.Data"
DotNet-Pack "./src/CUBRID.Data.Native.linux-x64"
DotNet-Pack "./src/CUBRID.Data.Native.win-x64"
DotNet-Pack "./src/CUBRID.Data.Native.win-x86"