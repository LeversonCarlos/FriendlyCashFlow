import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ElesseSharedModule } from '@elesse/shared';
import { ElesseIdentityRouting } from './elesse-identity.routing';
import { ElesseIdentityComponent } from './elesse-identity.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { LogoHeaderComponent } from './logo-header/logo-header.component';

@NgModule({
   declarations: [ElesseIdentityComponent,
      RegisterComponent, LoginComponent, LogoutComponent, ChangePasswordComponent, LogoHeaderComponent
   ],
   imports: [
      CommonModule, ElesseSharedModule,
      ElesseIdentityRouting
   ],
   exports: [],
   bootstrap: [ElesseIdentityComponent]
})
export class ElesseIdentityModule { }
