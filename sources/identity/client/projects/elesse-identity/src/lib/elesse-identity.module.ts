import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ElesseIdentityComponent } from './elesse-identity.component';
import { ElesseIdentityRouting } from './elesse-identity.routing';
import { RegisterComponent } from './register/register.component';

@NgModule({
   declarations: [ElesseIdentityComponent, RegisterComponent],
   imports: [
      CommonModule, FormsModule, ReactiveFormsModule, HttpClientModule,
      ElesseIdentityRouting
   ],
   exports: [ElesseIdentityComponent]
})
export class ElesseIdentityModule { }
