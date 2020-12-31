import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRouting } from './app-routing';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';

import { ElesseSharedModule, BusyService, MessageService, TokenService, SettingsService, InsightsService } from '@elesse/shared';
import { ElesseIdentityModule } from '@elesse/identity'

import { UrlInterceptorProvider } from './interceptors/url.interceptor';
import { ErrorInterceptorProvider } from './interceptors/error.interceptor';
import { RequestAuthInterceptorProvider, ResponseAuthInterceptorProvider } from './interceptors/auth.interceptor';
import { AnonymousHomeComponent } from './home/anonymous-home/anonymous-home.component';
import { AuthenticatedHomeComponent } from './home/authenticated-home/authenticated-home.component';
import { ContainerComponent } from './container/container.component';
import { AnonymousContainerComponent } from './container/anonymous-container/anonymous-container.component';

@NgModule({
   declarations: [
      AppComponent, HomeComponent, AnonymousHomeComponent, AuthenticatedHomeComponent, ContainerComponent, AnonymousContainerComponent
   ],
   imports: [
      BrowserModule, BrowserAnimationsModule, HttpClientModule, FormsModule, ReactiveFormsModule,
      ElesseSharedModule, ElesseIdentityModule, AppRouting
   ],
   providers: [SettingsService, InsightsService, MessageService,
      BusyService, TokenService,
      UrlInterceptorProvider, ErrorInterceptorProvider, RequestAuthInterceptorProvider, ResponseAuthInterceptorProvider
   ],
   bootstrap: [AppComponent]
})
export class AppModule { }
