import { NgModule } from '@angular/core';
import { ElesseSharedModule } from 'elesse-shared';
import { ElesseIdentityComponent } from './elesse-identity.component';
import { ElesseIdentityRouting } from './elesse-identity.routing';
import { RegisterComponent } from './register/register.component';

@NgModule({
   declarations: [ElesseIdentityComponent, RegisterComponent],
   imports: [
      ElesseSharedModule,
      ElesseIdentityRouting
   ],
   exports: [ElesseIdentityComponent]
})
export class ElesseIdentityModule { }
