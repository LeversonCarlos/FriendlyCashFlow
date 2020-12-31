import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input'
import { MatButtonModule } from '@angular/material/button';

import { ElesseSharedModule } from '@elesse/shared';
import { ElesseIdentityComponent } from './elesse-identity.component';
import { ElesseIdentityRouting } from './elesse-identity.routing';
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
      CommonModule,
      MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule,
      ElesseSharedModule,
      ElesseIdentityRouting
   ],
   exports: [
      MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule
   ],
   bootstrap: [ElesseIdentityComponent]
})
export class ElesseIdentityModule { }
