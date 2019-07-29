import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRouting } from './app.routing';
import { AppComponent } from './app.component';
import { baseUrlProvider, UrlInterceptorProvider } from './core/interceptors/url.interceptor';
import { CoreModule } from './core/core.module';

@NgModule({
   declarations: [
      AppComponent
   ],
   imports: [
      BrowserModule,
      AppRouting, CoreModule
   ],
   providers: [baseUrlProvider, UrlInterceptorProvider],
   bootstrap: [AppComponent]
})
export class AppModule { }
