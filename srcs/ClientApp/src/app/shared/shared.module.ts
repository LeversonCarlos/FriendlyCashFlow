import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

import { BaseLayoutComponent } from './base-layout/base-layout.component';
import { FullLayoutComponent } from './full-layout/full-layout.component';

@NgModule({
   declarations: [BaseLayoutComponent, FullLayoutComponent],
   imports: [
      CommonModule,
      MatToolbarModule, MatIconModule, MatButtonModule
   ],
   exports: [
      MatToolbarModule, MatIconModule, MatButtonModule,
      BaseLayoutComponent, FullLayoutComponent
   ]
})
export class SharedModule { }
