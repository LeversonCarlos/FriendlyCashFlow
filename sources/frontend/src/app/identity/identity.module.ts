import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';

import { IdentityRouting } from './identity.routing';
import { IdentityComponent } from './identity.component';
import { RegisterComponent } from './register/register.component';

@NgModule({
   declarations: [
      IdentityComponent,
      RegisterComponent,
   ],
   imports: [
      MaterialModule, SharedModule,
      IdentityRouting
   ],
   bootstrap: [IdentityComponent]
})
export class IdentityModule { }
