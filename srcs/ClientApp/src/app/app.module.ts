import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';

import { AppRouting } from './app.routing';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { SecureModule } from './secure/secure.module';

@NgModule({
   declarations: [
      AppComponent
   ],
   imports: [
      BrowserModule, BrowserAnimationsModule,
      AppRouting, CoreModule, SecureModule
   ],
   bootstrap: [AppComponent]
})
export class AppModule { }
