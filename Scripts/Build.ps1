param (
    [string]$bundlePath
)

Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

$projectPath = "GraphAlgorithms\GraphAlgorithms.csproj"
$runtime = "win-x64"
$outputPath = $bundlePath + "\Exe"

dotnet publish $projectPath --configuration Release --output $outputPath --runtime $runtime --no-self-contained
py -m venv "$outputPath\.venv"

Copy-Item -Path "Doc" -Destination $bundlePath -Recurse
Copy-Item -Path "Examples" -Destination $bundlePath -Recurse
