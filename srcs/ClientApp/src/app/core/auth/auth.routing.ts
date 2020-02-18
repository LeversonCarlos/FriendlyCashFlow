import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthUnguard } from './auth.unguard';
import { AuthComponent } from './auth.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { ActivateComponent } from './activate/activate.component';


const routes: Routes = [
   {
      path: '', component: AuthComponent,
      children: [
         { path: 'signup', canActivate: [AuthUnguard], component: SignUpComponent },
         { path: 'signin', canActivate: [AuthUnguard], component: SignInComponent },
         { path: 'activate/:id/:code', component: ActivateComponent },
      ]
   }
];

@NgModule({
   imports: [RouterModule.forChild(routes)],
   exports: [RouterModule]
})
export class AuthRouting { }
