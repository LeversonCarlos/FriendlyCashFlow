import { Routes } from '@angular/router';
import { AuthGuard } from '../core/auth/auth.guard';
import { EntriesFlowComponent } from './entries/entries-flow/entries-flow.component';
import { EntryDetailsComponent } from './entries/entry-details/entry-details.component';
import { TransferDetailsComponent } from './entries/transfer-details/transfer-details.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SettingsComponent } from './settings/settings.component';

export const secureRoutes: Routes = [
   { path: 'home', canActivate: [AuthGuard], component: DashboardComponent },
   { path: 'accounts', canActivate: [AuthGuard], loadChildren: './secure/accounts/accounts.module#AccountsModule' },
   { path: 'categories', canActivate: [AuthGuard], loadChildren: './secure/categories/categories.module#CategoriesModule' },
   { path: 'entries/flow/:year/:month/:account', canActivate: [AuthGuard], component: EntriesFlowComponent },
   { path: 'entries/flow', canActivate: [AuthGuard], component: EntriesFlowComponent },
   { path: 'entries', canActivate: [AuthGuard], component: EntriesFlowComponent },
   { path: 'entries/entry/new/:type', canActivate: [AuthGuard], component: EntryDetailsComponent },
   { path: 'entries/entry/:id', canActivate: [AuthGuard], component: EntryDetailsComponent },
   { path: 'transfer/:id', canActivate: [AuthGuard], component: TransferDetailsComponent },
   { path: 'analytics', canActivate: [AuthGuard], loadChildren: './secure/analytics/analytics.module#AnalyticsModule'  },
   { path: 'settings', canActivate: [AuthGuard], component: SettingsComponent },
];
