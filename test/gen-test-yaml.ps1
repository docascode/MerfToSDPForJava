$ErrorActionPreference = "Stop"

Write-Host "Start generating yamls for test ..."

$repoRoot = $($MyInvocation.MyCommand.Definition) | Split-Path | Split-Path

& "$repoRoot/src/MerfToSDPForJava/bin/Release/netcoreapp3.1/MerfToSDPForJava.exe" -s $repoRoot\test\Merf -o $repoRoot\test\ymldemo

Write-Host "Finished. Yamls generated in $repoRoot\test\ymldemo"
