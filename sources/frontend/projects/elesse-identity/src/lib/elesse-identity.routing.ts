import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard, UnauthGuard } from '@elesse/shared';
import { ElesseIdentityComponent } from './elesse-identity.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { ChangePasswordComponent } from './change-password/change-password.component';

export const ElesseIdentityRoutes: Routes = [
   { path: 'identity', loadChildren: () => import('./elesse-identity.module').then(m => m.ElesseIdentityModule) }
];

const internalRoutes: Routes = [
   {
      path: 'identity', component: ElesseIdentityComponent,
      children: [
         { path: '', redirectTo: 'login', pathMatch: 'full' },
         { path: 'register', canActivate: [UnauthGuard], component: RegisterComponent },
         { path: 'login', canActivate: [UnauthGuard], component: LoginComponent },
         { path: 'logout', canActivate: [AuthGuard], component: LogoutComponent },
         { path: 'change-password', canActivate: [AuthGuard], component: ChangePasswordComponent },
         // { path: 'activate/:id/:code', component: ActivateComponent },
      ]
   }
];

export const routerModuleForChild = RouterModule.forChild(internalRoutes)
@NgModule({
   imports: [routerModuleForChild],
   exports: [RouterModule]
})
export class ElesseIdentityRouting { }
