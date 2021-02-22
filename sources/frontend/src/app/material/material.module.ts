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
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSelectModule } from '@angular/material/select';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatBadgeModule } from '@angular/material/badge';

@NgModule({
   declarations: [],
   imports: [
      CommonModule,
      MatSnackBarModule, MatDialogModule, MatTooltipModule, MatMenuModule, MatProgressBarModule,
      MatCardModule, MatButtonModule,
      MatFormFieldModule, MatInputModule, MatSelectModule, MatAutocompleteModule,
      MatSidenavModule, MatToolbarModule, MatListModule, MatTabsModule,
      MatIconModule, MatDatepickerModule, MatMomentDateModule, MatSlideToggleModule, MatBadgeModule,
   ],
   exports: [
      MatSnackBarModule, MatDialogModule, MatTooltipModule, MatMenuModule, MatProgressBarModule,
      MatCardModule, MatButtonModule,
      MatFormFieldModule, MatInputModule, MatSelectModule, MatAutocompleteModule,
      MatSidenavModule, MatToolbarModule, MatListModule, MatTabsModule,
      MatIconModule, MatDatepickerModule, MatMomentDateModule, MatSlideToggleModule, MatBadgeModule,
   ]
})
export class MaterialModule { }
