import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRouting } from './app-routing';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import {
   ElesseSharedModule,
   BusyService, MessageService, TokenService, SettingsService,
   UrlInterceptorProvider, ErrorInterceptorProvider
} from 'elesse-shared';

@NgModule({
   declarations: [
      AppComponent
   ],
   imports: [
      BrowserModule, HttpClientModule, ElesseSharedModule,
      AppRouting
   ],
   providers: [SettingsService, MessageService, BusyService, TokenService,
      UrlInterceptorProvider, ErrorInterceptorProvider],
   bootstrap: [AppComponent]
})
export class AppModule { }
