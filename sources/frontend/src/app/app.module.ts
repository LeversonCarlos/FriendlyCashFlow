import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

import {
   SharedModule,
   SettingsService, BusyService, InsightsService, MessageService, ResponsiveService
} from '@elesse/shared';
import { MaterialModule } from '@elesse/material';
import { IdentityModule, TokenService } from '@elesse/identity';

import { AppRouting } from './app.routing';
import { MainComponent } from './main/main.component';
import { HomeComponent } from './main/home/home.component';
import { AnonymousHomeComponent } from './main/home/anonymous-home/anonymous-home.component';
import { AuthenticatedHomeComponent } from './main/home/authenticated-home/authenticated-home.component';

@NgModule({
   declarations: [
      MainComponent, HomeComponent, AnonymousHomeComponent, AuthenticatedHomeComponent
   ],
   imports: [
      CommonModule, BrowserModule, BrowserAnimationsModule, HttpClientModule,
      MaterialModule, SharedModule, IdentityModule,
      AppRouting,
   ],
   providers: [
      SettingsService, BusyService, InsightsService, MessageService, ResponsiveService,
      TokenService
   ],
   bootstrap: [MainComponent]
})
export class AppModule { }
