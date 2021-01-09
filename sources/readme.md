# Just a few commands to be remembered

## New frontend Module
- Create module with: 
   `ng g m Accounts --routing`
- Review the routing module name and file
   - accounts.routing.ts
   - AccountsRouting
- Edit the module and import the Material and Shared modules insted of Commom module
   ``` typescript
   import { MaterialModule } from '../material/material.exports';
   import { SharedModule } from '../shared/shared.exports';
   ```

## New front end Library
- Create project with:  
   `ng new ElesseIdentity --skip-git --skip--install --create-application=false`
- Add Library with:  
   `ng g library ElesseIdentity --prefix identity --skip-install`
