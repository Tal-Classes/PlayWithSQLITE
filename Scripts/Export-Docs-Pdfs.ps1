param(
    [string]$ProjectRoot = (Split-Path -Parent $PSScriptRoot)
)

$ErrorActionPreference = "Stop"

$scriptPath = Join-Path $PSScriptRoot "Export-Docs-Pdfs.py"

foreach ($candidate in @("python3", "python", "py")) {
    $cmd = Get-Command $candidate -CommandType Application -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($cmd -and (Test-Path -LiteralPath $scriptPath)) {
        $pyArgs = @()
        if ($candidate -eq "py") { $pyArgs += "-3" }
        $pyArgs += @($scriptPath)
        & $cmd.Source @pyArgs
        if ($LASTEXITCODE -eq 0) { exit 0 }
    }
}

Write-Error "Python is required to run Export-Docs-Pdfs.py"
exit 1

