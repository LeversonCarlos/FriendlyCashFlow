import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { SharedModule, SettingsService, BusyService, InsightsService, MessageService } from '@elesse/shared';
import { MaterialModule } from '@elesse/material';

import { AppRouting } from './app.routing';
import { RootComponent } from './root/root.component';

@NgModule({
   declarations: [
      RootComponent
   ],
   imports: [
      BrowserModule, BrowserAnimationsModule,
      MaterialModule, SharedModule,
      AppRouting,
   ],
   providers: [
      SettingsService, BusyService, InsightsService, MessageService
   ],
   bootstrap: [RootComponent]
})
export class AppModule { }
