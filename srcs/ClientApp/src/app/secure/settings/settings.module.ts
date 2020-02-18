import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';

import { SettingsRouting } from './settings.routing';
import { SettingsComponent } from './settings.component';
import { PasswordChangeComponent } from './password-change/password-change.component';


@NgModule({
   declarations: [SettingsComponent, PasswordChangeComponent],
   imports: [
      CommonModule, SharedModule,
      SettingsRouting
   ]
})
export class SettingsModule { }
