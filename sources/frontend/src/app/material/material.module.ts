import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input'
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';

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
