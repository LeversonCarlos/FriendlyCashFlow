import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRouting } from './app-routing';
import { AppComponent } from './app.component';
import { ElesseSharedModule, BusyService, MessageService, TokenService, SettingsService } from 'elesse-shared';

@NgModule({
   declarations: [
      AppComponent
   ],
   imports: [
      BrowserModule, ElesseSharedModule,
      AppRouting
   ],
   providers: [SettingsService, MessageService, BusyService, TokenService],
   bootstrap: [AppComponent]
})
export class AppModule { }
