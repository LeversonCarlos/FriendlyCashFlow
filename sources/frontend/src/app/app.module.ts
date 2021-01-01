import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRouting } from './app.routing';
import { RootComponent } from './root/root.component';

@NgModule({
   declarations: [
      RootComponent
   ],
   imports: [
      BrowserModule,
      AppRouting
   ],
   providers: [],
   bootstrap: [RootComponent]
})
export class AppModule { }
