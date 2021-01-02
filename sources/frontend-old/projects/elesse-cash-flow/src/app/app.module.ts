import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

import { AppRouting } from './app-routing';
import { MainContainerComponent } from './main-container/main-container.component';
import { HomeComponent } from './home/home.component';
import { AnonymousHomeComponent } from './home/anonymous-home/anonymous-home.component';
import { AuthenticatedHomeComponent } from './home/authenticated-home/authenticated-home.component';

import { ElesseSharedModule, TokenService, ResponsiveService } from '@elesse/shared';
// import { ElesseIdentityModule } from '@elesse/identity'

import { UrlInterceptorProvider } from './interceptors/url.interceptor';
import { ErrorInterceptorProvider } from './interceptors/error.interceptor';
import { RequestAuthInterceptorProvider, ResponseAuthInterceptorProvider } from './interceptors/auth.interceptor';

@NgModule({
   declarations: [
      MainContainerComponent,
      HomeComponent, AnonymousHomeComponent, AuthenticatedHomeComponent
   ],
   imports: [
      BrowserModule, BrowserAnimationsModule, HttpClientModule,
      ElesseSharedModule, AppRouting
   ],
   providers: [
      TokenService, ResponsiveService,
      UrlInterceptorProvider, ErrorInterceptorProvider, RequestAuthInterceptorProvider, ResponseAuthInterceptorProvider
   ],
   bootstrap: [MainContainerComponent]
})
export class AppModule { }
