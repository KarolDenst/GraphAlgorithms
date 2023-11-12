param (
    [ValidateSet(
        "win-x64", "win-x86", "win-arm", "win-arm64",
        "linux-x64", "linux-musl-x64", "linux-arm", "linux-arm64",
        "osx-x64", "osx-arm64",
        "browser-wasm"
    )]
    [string]$runtime = "win-x64"
)

$scriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Definition
$parentDirectory = Split-Path -Parent $scriptDirectory
Set-Location $parentDirectory

# Define the configuration (Debug or Release)
$buildConfiguration = "Release"

# Build the solution
dotnet build --configuration $buildConfiguration

# Check if the build was successful
if ($LASTEXITCODE -eq 0) {
    Write-Host "Build completed successfully."
} else {
    Write-Host "Build failed. Check the error messages for details."
    exit
}

# Publish the project
# Use an array for the parts of the path
$projectPath = "GraphAlgorithms\GraphAlgorithms.csproj"
$outputPath = "publish"
dotnet publish $projectPath --configuration $buildConfiguration --output $outputPath --runtime $runtime --self-contained

Write-Host "Executable should be created in: $outputPath."
Set-Location $scriptDirectory