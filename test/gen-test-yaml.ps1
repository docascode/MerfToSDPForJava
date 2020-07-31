$ErrorActionPreference = "Stop"

Write-Host "Start generating yamls for test ..."

$repoRoot = $($MyInvocation.MyCommand.Definition) | Split-Path | Split-Path

& "$repoRoot/src/MerfToSDPForJava/bin/Release/netcoreapp3.1/MerfToSDPForJava.exe" -s $repoRoot\test\docs-ref-autogen -o $repoRoot\test\docs-ref-autogen-sdp-after

Write-Host "Finished. Yamls generated in $repoRoot\test\docs-ref-autogen_sdp_after"
