import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { SignUpComponent } from './auth/sign-up/sign-up.component';
import { SignInComponent } from './auth/sign-in/sign-in.component';
import { ActivateComponent } from './auth/activate/activate.component';

export const coreRoutes: Routes = [
   { path: '', redirectTo: 'home', pathMatch: 'full' },
   { path: 'home', component: HomeComponent },
   { path: 'signup', component: SignUpComponent },
   { path: 'signin', component: SignInComponent },
   { path: 'activate/:id/:code', component: ActivateComponent },
];
