import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { HttpClientModule } from '@angular/common/http';
import { baseUrlProvider, UrlInterceptorProvider } from './interceptors/url.interceptor';
import { ErrorInterceptorProvider } from './interceptors/error.interceptor';
import { SharedModule } from '../shared/shared.module';
import { SignUpComponent } from './auth/sign-up/sign-up.component';

@NgModule({
   declarations: [HomeComponent, SignUpComponent],
   imports: [
      CommonModule, HttpClientModule, SharedModule
   ],
   exports: [HttpClientModule],
   providers: [baseUrlProvider, UrlInterceptorProvider, ErrorInterceptorProvider]
})
export class CoreModule { }
