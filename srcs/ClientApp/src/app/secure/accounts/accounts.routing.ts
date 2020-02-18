import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AccountsComponent } from './accounts.component';
import { AccountsListComponent } from './accounts-list/accounts-list.component';
import { AccountDetailsComponent } from './account-details/account-details.component';

const routes: Routes = [
   {
      path: '', component: AccountsComponent,
      children: [
         { path: '', component: AccountsListComponent },
         { path: ':id', component: AccountDetailsComponent },
      ]
   }
];

@NgModule({
   imports: [RouterModule.forChild(routes)],
   exports: [RouterModule]
})
export class AccountsRouting { }
