import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DetailsComponent } from './details/details.component';

const routes: Routes = [
   { path: 'edit/:transfer', component: DetailsComponent },
   { path: 'new', component: DetailsComponent },
];

@NgModule({
   imports: [RouterModule.forChild(routes)],
   exports: [RouterModule]
})
export class TransfersRouting { }
