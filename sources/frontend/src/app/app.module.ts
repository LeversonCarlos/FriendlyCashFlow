import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRouting } from './app.routing';
import { RootComponent } from './root/root.component';

import { SettingsService, BusyService, MessageService } from './shared/shared.exports';

@NgModule({
   declarations: [
      RootComponent
   ],
   imports: [
      BrowserModule,
      AppRouting
   ],
   providers: [
      SettingsService, BusyService, MessageService
   ],
   bootstrap: [RootComponent]
})
export class AppModule { }
