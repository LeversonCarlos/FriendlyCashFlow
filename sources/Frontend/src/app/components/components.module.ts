import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { LayoutComponent } from './layout/layout.component';
import { ApiClient } from './api-client';

import { MatToolbarModule } from '@angular/material/toolbar';

import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@NgModule({
   declarations: [
      LayoutComponent
   ],
   imports: [
      CommonModule, HttpClientModule,
      MatToolbarModule,
      MatButtonModule, MatIconModule,
   ],
   exports: [
      LayoutComponent, HttpClientModule,
      MatButtonModule, MatIconModule,
   ],
   providers: [
      ApiClient
   ]
})
export class ComponentsModule { }
