import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input'
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';

@NgModule({
   declarations: [],
   imports: [
      CommonModule, FormsModule, ReactiveFormsModule,
      MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule,
      MatSidenavModule, MatListModule,
   ],
   exports: [
      CommonModule, FormsModule, ReactiveFormsModule,
      MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule,
      MatSidenavModule, MatListModule
   ]
})
export class ElesseSharedModule { }
