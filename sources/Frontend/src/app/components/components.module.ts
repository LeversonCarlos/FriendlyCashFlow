import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { LayoutComponent } from './layout/layout.component';
import { ApiClient } from './api-client';

@NgModule({
   declarations: [
      LayoutComponent
   ],
   imports: [
      CommonModule, HttpClientModule,
   ],
   exports: [
      LayoutComponent, HttpClientModule,
   ],
   providers: [
      ApiClient
   ]
})
export class ComponentsModule { }
