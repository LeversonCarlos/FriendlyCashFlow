import { Routes } from '@angular/router';

export const coreRoutes: Routes = [
   { path: 'auth', loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule) },
];
