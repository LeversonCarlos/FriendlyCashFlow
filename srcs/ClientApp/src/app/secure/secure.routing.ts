import { Routes } from '@angular/router';
import { AuthGuard } from '../core/auth/auth.guard';

export const secureRoutes: Routes = [
   { path: '', redirectTo: 'home', pathMatch: 'full' },
   { path: 'home', canActivate: [AuthGuard], loadChildren: './secure/dashboard/dashboard.module#DashboardModule' },
   { path: 'accounts', canActivate: [AuthGuard], loadChildren: './secure/accounts/accounts.module#AccountsModule' },
   { path: 'categories', canActivate: [AuthGuard], loadChildren: './secure/categories/categories.module#CategoriesModule' },
   { path: 'entries', canActivate: [AuthGuard], loadChildren: './secure/entries/entries.module#EntriesModule' },
   { path: 'analytics', canActivate: [AuthGuard], loadChildren: './secure/analytics/analytics.module#AnalyticsModule' },
   { path: 'settings', canActivate: [AuthGuard], loadChildren: './secure/settings/settings.module#SettingsModule' },
];
