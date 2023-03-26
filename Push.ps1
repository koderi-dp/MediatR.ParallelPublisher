$scriptName = $MyInvocation.MyCommand.Name
$artifacts = "./artifacts"

if ([string]::IsNullOrEmpty($Env:NUGET_API_KEY)) {
    Write-Host "${scriptName}: NUGET_API_KEY is empty or not set. Skipped pushing package(s)."
} else {
    Get-ChildItem $artifacts -Filter "*.nupkg" | ForEach-Object {
        Write-Host "$($scriptName): Pushing $($_.Name)"
        Write-Host "NUGET_URL: $Env:NUGET_URL"
        Write-Host "NUGET_API_KEY: $Env:NUGET_API_KEY"
        Write-Host "PACKAGE: $_.FullName"
        
        dotnet nuget push $_.FullName --source $Env:NUGET_URL --api-key $Env:NUGET_API_KEY
        if ($lastexitcode -ne 0) {
            throw ("Exec: " + $errorMessage)
        }
    }
}
