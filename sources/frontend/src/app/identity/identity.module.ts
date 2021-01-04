import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';
import { IdentityRouting } from './identity.routing';

@NgModule({
   declarations: [],
   imports: [
      MaterialModule, SharedModule,
      IdentityRouting
   ]
})
export class IdentityModule { }
