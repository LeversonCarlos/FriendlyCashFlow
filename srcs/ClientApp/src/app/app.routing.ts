import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { coreRoutes } from './core/core.routing';
import { secureRoutes } from './secure/secure.routing';

@NgModule({
   imports: [RouterModule.forRoot(coreRoutes), RouterModule.forRoot(secureRoutes)],
   exports: [RouterModule]
})
export class AppRouting { }
