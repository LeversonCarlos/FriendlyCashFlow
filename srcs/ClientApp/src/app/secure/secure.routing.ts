import { Routes } from '@angular/router';
import { AuthGuard } from '../core/auth/auth.guard';
import { AccountsComponent } from './accounts/accounts.component';
import { AccountDetailsComponent } from './accounts/account-details/account-details.component';
import { CategoriesComponent } from './categories/categories.component';
import { CategoryDetailsComponent } from './categories/category-details/category-details.component';
import { EntriesFlowComponent } from './entries/entries-flow/entries-flow.component';
import { EntryDetailsComponent } from './entries/entry-details/entry-details.component';
import { TransferDetailsComponent } from './entries/transfer-details/transfer-details.component';

export const secureRoutes: Routes = [
   { path: 'accounts', canActivate: [AuthGuard], component: AccountsComponent },
   { path: 'account/:id', canActivate: [AuthGuard], component: AccountDetailsComponent },
   { path: 'categories', canActivate: [AuthGuard], component: CategoriesComponent },
   { path: 'category/:id', canActivate: [AuthGuard], component: CategoryDetailsComponent },
   { path: 'entries/flow/:year/:month/:account', canActivate: [AuthGuard], component: EntriesFlowComponent },
   { path: 'entries/flow', canActivate: [AuthGuard], component: EntriesFlowComponent },
   { path: 'entries', canActivate: [AuthGuard], component: EntriesFlowComponent },
   { path: 'entries/entry/new/:type', canActivate: [AuthGuard], component: EntryDetailsComponent },
   { path: 'entries/entry/:id', canActivate: [AuthGuard], component: EntryDetailsComponent },
   { path: 'entries/transfer/:id', canActivate: [AuthGuard], component: TransferDetailsComponent },
];
