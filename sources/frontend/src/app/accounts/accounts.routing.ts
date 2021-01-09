import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ListComponent } from './list/list.component';

const routes: Routes = [
   { path: '', redirectTo: 'list', pathMatch: 'full' },
   { path: 'list', component: ListComponent },
   // { path: 'activate/:id/:code', component: ActivateComponent },
];

@NgModule({
   imports: [RouterModule.forChild(routes)],
   exports: [RouterModule]
})
export class AccountsRouting { }
