import { Routes } from '@angular/router';

export const coreRoutes: Routes = [
   { path: 'auth', loadChildren: './core/auth/auth.module#AuthModule' },
];
