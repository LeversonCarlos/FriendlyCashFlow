import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatToolbarModule } from '@angular/material/toolbar';
import { BaseLayoutComponent } from './base-layout/base-layout.component';

@NgModule({
   declarations: [BaseLayoutComponent],
   imports: [
      CommonModule,
      MatToolbarModule
   ],
   exports: [
      MatToolbarModule, BaseLayoutComponent
   ]
})
export class SharedModule { }
