import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatToolbarModule } from '@angular/material/toolbar';
import { BaseLayoutComponent } from './base-layout/base-layout.component';
import { FullLayoutComponent } from './full-layout/full-layout.component';

@NgModule({
   declarations: [BaseLayoutComponent, FullLayoutComponent],
   imports: [
      CommonModule,
      MatToolbarModule
   ],
   exports: [
      MatToolbarModule, BaseLayoutComponent, FullLayoutComponent
   ]
})
export class SharedModule { }
