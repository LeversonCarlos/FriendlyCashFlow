import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/core/auth/auth.guard';
import { AccountsComponent } from './accounts.component';
import { AccountsListComponent } from './accounts-list/accounts-list.component';
import { AccountDetailsComponent } from './account-details/account-details.component';

const routes: Routes = [
   {
      path: '', component: AccountsComponent,
      children: [
         { path: '', redirectTo: 'list', pathMatch: 'full' },
         { path: 'list', canActivate: [AuthGuard], component: AccountsListComponent },
         { path: ':id', canActivate: [AuthGuard], component: AccountDetailsComponent },
      ]
   }
];

@NgModule({
   imports: [RouterModule.forChild(routes)],
   exports: [RouterModule]
})
export class AccountsRouting { }
