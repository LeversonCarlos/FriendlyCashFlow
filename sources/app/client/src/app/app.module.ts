import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { ElesseIdentityModule } from 'elesse-identity';

@NgModule({
   declarations: [
      AppComponent
   ],
   imports: [
      BrowserModule,
      AppRoutingModule, ElesseIdentityModule
   ],
   providers: [],
   bootstrap: [AppComponent]
})
export class AppModule { }
