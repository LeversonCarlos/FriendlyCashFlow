import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { HttpClientModule } from '@angular/common/http';
import { baseUrlProvider, UrlInterceptorProvider } from './interceptors/url.interceptor';
import { ErrorInterceptorProvider } from './interceptors/error.interceptor';
import { SharedModule } from '../shared/shared.module';
import { SignUpComponent } from './auth/sign-up/sign-up.component';
import { SignInComponent } from './auth/sign-in/sign-in.component';
import { AuthService } from './auth/auth.service';
import { ActivateComponent } from './auth/activate/activate.component';
import { RequestAuthInterceptorProvider, ResponseAuthInterceptorProvider } from './interceptors/auth.interceptor';

@NgModule({
   declarations: [HomeComponent, SignUpComponent, SignInComponent, ActivateComponent],
   imports: [
      CommonModule, HttpClientModule, SharedModule
   ],
   exports: [HttpClientModule],
   providers: [baseUrlProvider, UrlInterceptorProvider, RequestAuthInterceptorProvider, ErrorInterceptorProvider, ResponseAuthInterceptorProvider, AuthService]
})
export class CoreModule { }
