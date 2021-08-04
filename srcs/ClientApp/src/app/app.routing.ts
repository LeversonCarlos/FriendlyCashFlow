import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { coreRoutes } from './core/core.routing';
import { secureRoutes } from './secure/secure.routing';

@NgModule({
   imports: [RouterModule.forRoot(coreRoutes, { relativeLinkResolution: 'legacy' }), RouterModule.forRoot(secureRoutes, { relativeLinkResolution: 'legacy' })],
   exports: [RouterModule]
})
export class AppRouting { }
