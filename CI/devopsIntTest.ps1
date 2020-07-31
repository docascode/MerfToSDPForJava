$ErrorActionPreference = "Stop"

Write-Host "Start integration test..."
$repoRoot = $($MyInvocation.MyCommand.Definition) | Split-Path | Split-Path
& "$repoRoot/src/MerfToSDPForJava/bin/Release/netcoreapp3.1/MerfToSDPForJava.exe" -s $repoRoot\test\docs-ref-autogen -o $repoRoot\test\docs-ref-autogen-sdp-after
& "$repoRoot/test/DiffFiles/bin/Release/netcoreapp3.1/DiffFiles.exe" -o $repoRoot\test\docs-ref-autogen-sdp\SDPYml -n $repoRoot\test\docs-ref-autogen-sdp-after\SDPYml -l $repoRoot\test --Path
if ($LASTEXITCODE -ne 0)
{
    Write-Error "Diff found for SDP yml."
}
else {
    Write-Host "Done testing SDP yml..."
}

Write-Host "Finished integration test..."
