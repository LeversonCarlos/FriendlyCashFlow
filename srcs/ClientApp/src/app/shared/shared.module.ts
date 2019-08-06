import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import 'hammerjs';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialogModule } from '@angular/material/dialog';

import { BaseLayoutComponent } from './base-layout/base-layout.component';
import { FullLayoutComponent } from './full-layout/full-layout.component';
import { SideNavComponent } from './side-nav/side-nav.component';
import { BusyComponent } from './busy/busy.component';
import { BusyService } from './busy/busy.service';
import { TranslationService } from './translation/translation.service';
import { TranslationPipe } from './translation/translation.pipe';
import { MessageComponent } from './message/message.component';
import { MessageService } from './message/message.service';
import { ConfirmComponent } from './message/confirm.component';
import { FabButtonComponent } from './fab-button/fab-button.component';
import { RelatedBoxComponent } from './related-box/related-box.component';

@NgModule({
   entryComponents: [MessageComponent, ConfirmComponent],
   declarations: [BaseLayoutComponent, FullLayoutComponent, SideNavComponent,
      BusyComponent, TranslationPipe, MessageComponent, ConfirmComponent, FabButtonComponent, RelatedBoxComponent],
   imports: [
      CommonModule, RouterModule, FormsModule, ReactiveFormsModule,
      MatToolbarModule, MatSidenavModule, MatCardModule, MatTabsModule, MatListModule,
      MatIconModule, MatButtonModule, MatProgressBarModule,
      MatFormFieldModule, MatInputModule, MatAutocompleteModule, MatSelectModule, MatSlideToggleModule,
      MatSnackBarModule, MatDialogModule
   ],
   exports: [
      RouterModule, FormsModule, ReactiveFormsModule,
      MatToolbarModule, MatSidenavModule, MatCardModule, MatTabsModule, MatListModule,
      MatIconModule, MatButtonModule, MatProgressBarModule,
      MatFormFieldModule, MatInputModule, MatAutocompleteModule, MatSelectModule, MatSlideToggleModule,
      BaseLayoutComponent, FullLayoutComponent,
      FabButtonComponent, RelatedBoxComponent,
      TranslationPipe
   ],
   providers: [BusyService, TranslationService, MessageService]
})
export class SharedModule { }
