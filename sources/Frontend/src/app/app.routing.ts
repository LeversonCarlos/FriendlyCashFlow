import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FrontendRoute as AccountRoute } from '@models/accounts';

const routes: Routes = [
   { path: AccountRoute, loadChildren: () => import('./modules/accounts/accounts.module').then(m => m.AccountsModule) },
];

@NgModule({
   imports: [RouterModule.forRoot(routes)],
   exports: [RouterModule]
})
export class AppRoutingModule { }
