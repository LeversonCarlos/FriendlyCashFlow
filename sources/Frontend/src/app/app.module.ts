import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app.routing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';

@NgModule({
   declarations: [
      AppComponent
   ],
   imports: [
      BrowserModule, BrowserAnimationsModule,
      AppRoutingModule,
   ],
   providers: [],
   bootstrap: [AppComponent]
})
export class AppModule { }
