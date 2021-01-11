import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input'
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialogModule } from '@angular/material/dialog';

@NgModule({
   declarations: [],
   imports: [
      CommonModule,
      MatSnackBarModule, MatDialogModule,
      MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule,
      MatSidenavModule, MatToolbarModule, MatListModule, MatTabsModule,
      MatIconModule,
   ],
   exports: [
      MatSnackBarModule, MatDialogModule,
      MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule,
      MatSidenavModule, MatToolbarModule, MatListModule, MatTabsModule,
      MatIconModule,
   ]
})
export class MaterialModule { }
