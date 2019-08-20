import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { SignUpComponent } from './auth/sign-up/sign-up.component';
import { SignInComponent } from './auth/sign-in/sign-in.component';
import { ActivateComponent } from './auth/activate/activate.component';
import { AuthGuard } from './auth/auth.guard';
import { AuthUnguard } from './auth/auth.unguard';

export const coreRoutes: Routes = [
   { path: '', redirectTo: 'home', pathMatch: 'full' },
   { path: 'home', canActivate: [AuthGuard], component: HomeComponent },
   { path: 'signup', canActivate: [AuthUnguard], component: SignUpComponent },
   { path: 'signin', canActivate: [AuthUnguard], component: SignInComponent },
   { path: 'activate/:id/:code', component: ActivateComponent },
];
