import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { UnauthGuard } from './guards/unauth.guard';
import { IdentityComponent } from './identity.component';
import { LogoutComponent } from './logout/logout.component';
import { RegisterComponent } from './register/register.component';

const routes: Routes = [{
   path: '', component: IdentityComponent,
   children: [
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'register', canActivate: [UnauthGuard], component: RegisterComponent },
      // { path: 'login', canActivate: [UnauthGuard], component: LoginComponent },
      { path: 'logout', canActivate: [AuthGuard], component: LogoutComponent },
      // { path: 'change-password', canActivate: [AuthGuard], component: ChangePasswordComponent },
      // { path: 'activate/:id/:code', component: ActivateComponent },
   ]
}];

@NgModule({
   imports: [RouterModule.forChild(routes)],
   exports: [RouterModule]
})
export class IdentityRouting { }
