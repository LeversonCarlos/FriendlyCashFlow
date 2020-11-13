import { NgModule } from '@angular/core';
import { ElesseIdentityComponent } from './elesse-identity.component';
import { RegisterComponent } from './register/register.component';

@NgModule({
   declarations: [ElesseIdentityComponent, RegisterComponent],
   imports: [
   ],
   exports: [ElesseIdentityComponent]
})
export class ElesseIdentityModule { }
