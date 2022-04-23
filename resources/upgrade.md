# steps to upgrade each angular version
- `ng update @angular/cli@10`
- `ng update @angular/core@10 @angular/cli@10`
- `ng update @angular/material@10`
- `ng update @angular/pwa@0.1002`

# steps to upgrade global angular version
- `npm uninstall -g @angular-cli`
- `npm install -g @angular/cli@10`

# steps to upgrade only patch (or use minor on target parameter) versions
- `npx npm-check-updates --upgrade --target "patch" --filter "/@angular.*/"`
- `npx npm-check-updates --upgrade --target "patch" --filter "/@microsoft.*/"`
- `npx npm-check-updates --upgrade --target "patch" --filter "highcharts,moment,rxjs"`
- `npx npm-check-updates --upgrade --target "patch" --filter "typescript,tslib,tslint,ts-node,@types/node,zone.js"`
