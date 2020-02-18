import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EntriesComponent } from './entries.component';
import { EntriesFlowComponent } from './entries-flow/entries-flow.component';
import { EntryDetailsComponent } from './entry-details/entry-details.component';
import { TransferDetailsComponent } from './transfer-details/transfer-details.component';

const routes: Routes = [
   {
      path: '', component: EntriesComponent,
      children: [
         { path: '', redirectTo: 'flow', pathMatch: 'full' },
         { path: 'flow/:year/:month/:account', component: EntriesFlowComponent },
         { path: 'flow', component: EntriesFlowComponent },
         { path: 'entry/new/:type', component: EntryDetailsComponent },
         { path: 'entry/:id', component: EntryDetailsComponent },
         { path: 'transfer/:id', component: TransferDetailsComponent },
      ]
   }
];

@NgModule({
   imports: [RouterModule.forChild(routes)],
   exports: [RouterModule]
})
export class EntriesRouting { }
