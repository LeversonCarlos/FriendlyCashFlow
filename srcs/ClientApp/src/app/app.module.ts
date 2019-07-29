import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRouting } from './app.routing';
import { AppComponent } from './app.component';

@NgModule({
   declarations: [
      AppComponent
   ],
   imports: [
      /* BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }), */
      BrowserModule,
      AppRouting
   ],
   providers: [],
   bootstrap: [AppComponent]
})
export class AppModule { }
