import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ElesseIdentityComponent } from './elesse-identity.component';
import { RegisterComponent } from './register/register.component';

export const ElesseIdentityRoutes: Routes = [
   { path: 'identity', loadChildren: () => import('./elesse-identity.module').then(m => m.ElesseIdentityModule) }
];

const internalRoutes: Routes = [
   {
      path: '', component: ElesseIdentityComponent,
      children: [
         { path: 'register', component: RegisterComponent },
         // { path: 'signin', canActivate: [AuthUnguard], component: SignInComponent },
         // { path: 'activate/:id/:code', component: ActivateComponent },
      ]
   }
];

@NgModule({
   imports: [RouterModule.forChild(internalRoutes)],
   exports: [RouterModule]
})
export class ElesseIdentityRouting { }
