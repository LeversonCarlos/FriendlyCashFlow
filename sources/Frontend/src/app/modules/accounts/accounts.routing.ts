import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ViewRoutes } from '@models/accounts';
import { ContainerComponent } from './views/container.component';
import { IndexComponent } from './views/index/index.component';

const routes: Routes = [
   {
      path: '', component: ContainerComponent,
      children: [
         { path: '', redirectTo: ViewRoutes.Index, pathMatch: 'full' },
         { path: ViewRoutes.Index, component: IndexComponent },
      ]
   }
];

@NgModule({
   imports: [RouterModule.forChild(routes)],
   exports: [RouterModule]
})
export class AccountsRouting { }
