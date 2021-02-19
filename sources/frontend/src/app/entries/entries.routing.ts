import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DetailsComponent } from './details/details.component';

const routes: Routes = [
   { path: 'edit/:entry', component: DetailsComponent },
   { path: 'new/income', component: DetailsComponent },
   { path: 'new/expense', component: DetailsComponent },
];

@NgModule({
   imports: [RouterModule.forChild(routes)],
   exports: [RouterModule]
})
export class EntriesRouting { }
