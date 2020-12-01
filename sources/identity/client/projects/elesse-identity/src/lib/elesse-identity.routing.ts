import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard, UnauthGuard } from 'elesse-shared';
import { ElesseIdentityComponent } from './elesse-identity.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { RegisterComponent } from './register/register.component';

export const ElesseIdentityRoutes: Routes = [
   { path: 'identity', loadChildren: () => import('./elesse-identity.module').then(m => m.ElesseIdentityModule) }
];

const internalRoutes: Routes = [
   {
      path: '', component: ElesseIdentityComponent,
      children: [
         { path: 'register', canActivate: [UnauthGuard], component: RegisterComponent },
         { path: 'login', canActivate: [UnauthGuard], component: LoginComponent },
         { path: 'logout', canActivate: [AuthGuard], component: LogoutComponent },
         // { path: 'activate/:id/:code', component: ActivateComponent },
      ]
   }
];

@NgModule({
   imports: [RouterModule.forChild(internalRoutes)],
   exports: [RouterModule]
})
export class ElesseIdentityRouting { }
