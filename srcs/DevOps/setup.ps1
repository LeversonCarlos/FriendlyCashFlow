Write-Output ""
Write-Output "> DOTNET VERSION >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
dotnet --version

Write-Output ""
Write-Output "> DOTNET TOOLS VERSION >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
dotnet tool list --global

Write-Output ""
Write-Output "> NODE VERSION >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
node --version

Write-Output ""
Write-Output "> NPM VERSION >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
npm --version
npm config list

Write-Output ""
Write-Output "> INSTALL EF >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
dotnet tool install --global dotnet-ef --version 3.1.1-*

Write-Output ""
Write-Output "> DOTNET TOOLS VERSION >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
dotnet tool list --global

Write-Output ""
Write-Output "> DONE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"
