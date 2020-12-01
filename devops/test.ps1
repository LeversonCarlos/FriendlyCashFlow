Write-Host "> Shared.Client"
cd ../sources/shared/client
## npm install
npm run build
# npm run test
cd ../../../devops


Write-Host "> Identity.Client"
cd ../sources/identity/client
## npm install
npm run build
# npm run test
cd ../../../devops


Write-Host "> Application.Client"
cd ../sources/app/client
## npm install
npm run build
# npm run test
cd ../../../devops
