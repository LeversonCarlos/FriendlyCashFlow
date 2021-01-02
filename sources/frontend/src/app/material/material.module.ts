import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
   MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule,
   MatSidenavModule, MatListModule,
} from './material.exports';

@NgModule({
   declarations: [],
   imports: [
      CommonModule,
      MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule,
      MatSidenavModule, MatListModule,
   ],
   exports: [
      MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule,
      MatSidenavModule, MatListModule,
   ]
})
export class MaterialModule { }
