import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRouting } from './app-routing';
import { AppComponent } from './app.component';
import { ElesseSharedModule, BusyService, MessageService, TokenService, SettingsService, InsightsService } from 'elesse-shared';

import { UrlInterceptorProvider } from './interceptors/url.interceptor';
import { ErrorInterceptorProvider } from './interceptors/error.interceptor';
import { RequestAuthInterceptorProvider, ResponseAuthInterceptorProvider } from './interceptors/auth.interceptor';

@NgModule({
   declarations: [
      AppComponent
   ],
   imports: [
      BrowserModule, HttpClientModule, ElesseSharedModule,
      AppRouting
   ],
   providers: [SettingsService, InsightsService, MessageService,
      BusyService, TokenService,
      UrlInterceptorProvider, ErrorInterceptorProvider, RequestAuthInterceptorProvider, ResponseAuthInterceptorProvider
   ],
   bootstrap: [AppComponent]
})
export class AppModule { }
