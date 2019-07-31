import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import 'hammerjs';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

import { BaseLayoutComponent } from './base-layout/base-layout.component';
import { FullLayoutComponent } from './full-layout/full-layout.component';

@NgModule({
   declarations: [BaseLayoutComponent, FullLayoutComponent],
   imports: [
      CommonModule,
      MatToolbarModule, MatSidenavModule, MatListModule, MatIconModule, MatButtonModule
   ],
   exports: [
      MatToolbarModule, MatSidenavModule, MatListModule, MatIconModule, MatButtonModule,
      BaseLayoutComponent, FullLayoutComponent
   ]
})
export class SharedModule { }
