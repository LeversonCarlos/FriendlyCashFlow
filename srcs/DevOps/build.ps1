param([String]$version='1.0.0', [String]$output='./bin/Publish', [String]$configuration='Release')

## PARAMETERS ##
Write-Output ""
Write-Output "> PARAMETERS >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
Write-Output "  version: $version"
Write-Output "  output: $output"
Write-Output "  configuration: $configuration"
$currentPath = (Get-Location).tostring()
   Write-Output "  currentPath: $currentPath"

## TOOLS VERSION ##
Write-Output ""
Write-Output "> TOOLS VERSION >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
   dotnet --version
   dotnet ef --version
   npm --version

## OUTPUT PATH ##
Write-Output ""
Write-Output "> OUTPUT PATH >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
New-Item -ItemType Directory -Force -Path $output | Out-Null
Get-ChildItem -Path $output -Include * -File -Recurse | Remove-Item | Out-Null
$output = (Resolve-Path $output).tostring()
   Write-Output "  publishPath: $output"

## APPLY VERSION ##
Write-Output ""
Write-Output "> APPLY VERSION >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
$donetProjectFile = 'FriendlyCashFlow.csproj'
   Write-Output "  donetProjectFile: $donetProjectFile"
   (Get-Content $donetProjectFile) | % { $_ -replace '<Version>1.0.0</Version>', "<Version>$version</Version>" } | Set-Content $donetProjectFile
   #echo (Get-Content $donetProjectFile)
$angularProjectFile = 'ClientApp/package.json'
   Write-Output "  angularProjectFile: $angularProjectFile"
   (Get-Content $angularProjectFile) | %{ $_ -replace '"version": "1.0.0"', "@@version@@: @@$version@@" } | %{ $_ -replace '@@', '"' } | Set-Content $angularProjectFile
   #echo (Get-Content $angularProjectFile)
# $angularManifestFile = 'ClientApp/src/manifest.json'
#    Write-Output "  angularManifestFile: $angularManifestFile"
#    (Get-Content $angularManifestFile) | %{ $_ -replace '"version": "1.0.0"', "@@version@@: @@$version@@" } | %{ $_ -replace '@@', '"' } | Set-Content $angularManifestFile
#    #echo (Get-Content $angularManifestFile)

## BACKEND PACKAGES ##
Write-Output ""
Write-Output "> BACKEND PACKAGES >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
dotnet restore

## FRONTEND PACKAGES ##
# Write-Output ""
# Write-Output "> FRONTEND PACKAGES >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
# cd ClientApp
## npx npm-force-resolutions
# npm install --no-save -verbose
# cd..

## BUILDING ##
Write-Output ""
Write-Output "> BUILDING >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
dotnet build --configuration $configuration --no-restore

## PUBLISHING ##
Write-Output ""
Write-Output "> PUBLISHING >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
dotnet publish --configuration $configuration --output $output/build --no-restore --no-build

## MIGRATIONS ##
Write-Output ""
Write-Output "> MIGRATIONS >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
dotnet ef migrations script -o $output/scripts/migrations.sql -i -v

## CLEANING ##
Write-Output ""
Write-Output "> CLEANING >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
Get-ChildItem -Path $output -Include appsettings.Development.json -File -Recurse | Remove-Item
Get-ChildItem -Path $output -Include *.pdb -File -Recurse | Remove-Item
