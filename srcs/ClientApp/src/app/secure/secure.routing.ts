import { Routes } from '@angular/router';
import { AuthGuard } from '../core/auth/auth.guard';

export const secureRoutes: Routes = [
   { path: '', redirectTo: 'home', pathMatch: 'full' },
   { path: 'home', canActivate: [AuthGuard], loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule) },
   { path: 'accounts', canActivate: [AuthGuard], loadChildren: () => import('./accounts/accounts.module').then(m => m.AccountsModule) },
   { path: 'categories', canActivate: [AuthGuard], loadChildren: () => import('./categories/categories.module').then(m => m.CategoriesModule) },
   { path: 'entries', canActivate: [AuthGuard], loadChildren: () => import('./entries/entries.module').then(m => m.EntriesModule) },
   { path: 'analytics', canActivate: [AuthGuard], loadChildren: () => import('./analytics/analytics.module').then(m => m.AnalyticsModule) },
   { path: 'settings', canActivate: [AuthGuard], loadChildren: () => import('./settings/settings.module').then(m => m.SettingsModule) },
];
