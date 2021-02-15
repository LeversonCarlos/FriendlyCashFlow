import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

import {
   SharedModule,
   SettingsService, BusyService, InsightsService, MessageService,
   ResponsiveService, LocalizationService, MonthSelectorService,
} from '@elesse/shared';
import { MaterialModule } from '@elesse/material';
import { IdentityModule, TokenService } from '@elesse/identity';

import { AppRouting } from './app.routing';
import { UrlInterceptorProvider } from './main/interceptors/url.interceptor';
import { LangInterceptorProvider } from './main/interceptors/lang.interceptor';
import { ErrorInterceptorProvider } from './main/interceptors/error.interceptor';
import { RequestAuthInterceptorProvider, ResponseAuthInterceptorProvider } from './main/interceptors/auth.interceptor';

import { MainComponent } from './main/main.component';
import { HomeComponent } from './main/home/home.component';
import { AnonymousHomeComponent } from './main/home/anonymous-home/anonymous-home.component';
import { AuthenticatedHomeComponent } from './main/home/authenticated-home/authenticated-home.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { MainData } from './main/data/data.service';
import { AccountsProviders } from '@elesse/accounts';
import { EntriesProviders } from '@elesse/entries';


@NgModule({
   declarations: [
      MainComponent, HomeComponent, AnonymousHomeComponent, AuthenticatedHomeComponent
   ],
   imports: [
      CommonModule, BrowserModule, BrowserAnimationsModule, HttpClientModule,
      MaterialModule, SharedModule, IdentityModule,
      AppRouting,
      ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production, registrationStrategy: 'registerImmediately' }),
   ],
   providers: [
      SettingsService, BusyService, InsightsService, MessageService,
      ResponsiveService, LocalizationService,
      TokenService,
      UrlInterceptorProvider, LangInterceptorProvider, ErrorInterceptorProvider, RequestAuthInterceptorProvider, ResponseAuthInterceptorProvider,
      MainData, MonthSelectorService, AccountsProviders, EntriesProviders
   ],
   bootstrap: [MainComponent]
})
export class AppModule { }
