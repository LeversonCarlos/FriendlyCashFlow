import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRouting } from './app.routing';
import { RootComponent } from './root/root.component';

import { SettingsService, BusyService, InsightsService, MessageService } from './shared/shared.exports';

@NgModule({
   declarations: [
      RootComponent
   ],
   imports: [
      BrowserModule, BrowserAnimationsModule,
      AppRouting,
   ],
   providers: [
      SettingsService, BusyService, InsightsService, MessageService
   ],
   bootstrap: [RootComponent]
})
export class AppModule { }
