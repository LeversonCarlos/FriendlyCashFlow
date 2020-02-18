import { Routes } from '@angular/router';
import { AuthGuard } from '../core/auth/auth.guard';
import { CategoriesComponent } from './categories/categories.component';
import { CategoryDetailsComponent } from './categories/category-details/category-details.component';
import { EntriesFlowComponent } from './entries/entries-flow/entries-flow.component';
import { EntryDetailsComponent } from './entries/entry-details/entry-details.component';
import { TransferDetailsComponent } from './entries/transfer-details/transfer-details.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SettingsComponent } from './settings/settings.component';
import { AnalyticsComponent } from './analytics/analytics.component';

export const secureRoutes: Routes = [
   { path: 'home', canActivate: [AuthGuard], component: DashboardComponent },
   { path: 'accounts', canActivate: [AuthGuard], loadChildren: './secure/accounts/accounts.module#AccountsModule' },
   { path: 'categories', canActivate: [AuthGuard], component: CategoriesComponent },
   { path: 'category/:id', canActivate: [AuthGuard], component: CategoryDetailsComponent },
   { path: 'entries/flow/:year/:month/:account', canActivate: [AuthGuard], component: EntriesFlowComponent },
   { path: 'entries/flow', canActivate: [AuthGuard], component: EntriesFlowComponent },
   { path: 'entries', canActivate: [AuthGuard], component: EntriesFlowComponent },
   { path: 'entries/entry/new/:type', canActivate: [AuthGuard], component: EntryDetailsComponent },
   { path: 'entries/entry/:id', canActivate: [AuthGuard], component: EntryDetailsComponent },
   { path: 'transfer/:id', canActivate: [AuthGuard], component: TransferDetailsComponent },
   { path: 'analytics/:year/:month', canActivate: [AuthGuard], component: AnalyticsComponent },
   { path: 'analytics', canActivate: [AuthGuard], component: AnalyticsComponent },
   { path: 'settings', canActivate: [AuthGuard], component: SettingsComponent },
];
