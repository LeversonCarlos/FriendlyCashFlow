import { NgModule } from '@angular/core';
import { ElesseSharedModule } from 'elesse-shared';
import { ElesseIdentityComponent } from './elesse-identity.component';
import { ElesseIdentityRouting } from './elesse-identity.routing';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { ChangePasswordComponent } from './change-password/change-password.component';

@NgModule({
   declarations: [ElesseIdentityComponent,
      RegisterComponent, LoginComponent, LogoutComponent, ChangePasswordComponent
   ],
   imports: [
      ElesseSharedModule,
      ElesseIdentityRouting
   ],
   exports: [],
   bootstrap: [ElesseIdentityComponent]
})
export class ElesseIdentityModule { }
