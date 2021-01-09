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
- Add module into app routes with:
   ``` typescript
   { 
      path: 'accounts', 
      canActivate: [AuthGuard], 
      loadChildren: () => import('./accounts/accounts.module').then(m => m.AccountsModule) 
   },
   ```
- Create components with:
   `ng g c accounts/List --selector accounts-list`
- Add component to module routes with:
   ``` typescript
   const routes: Routes = [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: ListComponent },
   ];
   ```
- Import test module on the component test file with:
   ``` typescript
   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [ListComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   ```

## New front end Library
- Create project with:  
   `ng new ElesseIdentity --skip-git --skip--install --create-application=false`
- Add Library with:  
   `ng g library ElesseIdentity --prefix identity --skip-install`
