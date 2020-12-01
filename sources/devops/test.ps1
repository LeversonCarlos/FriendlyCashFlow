Write-Host "> Shared.Client"
cd ../shared/client
## npm install
npm run build
# npm run test
cd ../../devops


Write-Host "> Identity.Client"
cd ../identity/client
## npm install
npm run build
# npm run test
cd ../../devops


Write-Host "> Application.Client"
cd ../app/client
## npm install
npm run build
# npm run test
cd ../../devops
