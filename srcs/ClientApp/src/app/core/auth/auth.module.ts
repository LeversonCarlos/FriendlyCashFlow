
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';

import { AuthRouting } from './auth.routing';
import { AuthComponent } from './auth.component';
import { ActivateComponent } from './activate/activate.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { SignUpComponent } from './sign-up/sign-up.component';

@NgModule({
   declarations: [AuthComponent, ActivateComponent, SignInComponent, SignUpComponent],
   imports: [
      CommonModule, SharedModule,
      AuthRouting
   ]
})
export class AuthModule { }
