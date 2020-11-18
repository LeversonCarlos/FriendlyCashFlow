import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRouting } from './app-routing';
import { AppComponent } from './app.component';
import { BusyService, MessageService, TokenService } from 'elesse-shared';

@NgModule({
   declarations: [
      AppComponent
   ],
   imports: [
      BrowserModule,
      AppRouting
   ],
   providers: [MessageService, BusyService, TokenService],
   bootstrap: [AppComponent]
})
export class AppModule { }
