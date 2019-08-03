import { Routes } from '@angular/router';
import { AccountsComponent } from './accounts/accounts.component';
import { AccountDetailsComponent } from './accounts/account-details/account-details.component';

export const secureRoutes: Routes = [
   { path: 'accounts', component: AccountsComponent },
   { path: 'account/:id', component: AccountDetailsComponent },
];
