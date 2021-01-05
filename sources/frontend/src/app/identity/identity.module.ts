import { NgModule } from '@angular/core';
import { MaterialModule } from '../material/material.exports';
import { SharedModule } from '../shared/shared.exports';

import { IdentityRouting } from './identity.routing';
import { IdentityComponent } from './identity.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { LogoutComponent } from './logout/logout.component';

@NgModule({
   declarations: [
      IdentityComponent,
      RegisterComponent, LoginComponent, ChangePasswordComponent, LogoutComponent
   ],
   imports: [
      MaterialModule, SharedModule,
      IdentityRouting
   ],
   bootstrap: [IdentityComponent]
})
export class IdentityModule { }
