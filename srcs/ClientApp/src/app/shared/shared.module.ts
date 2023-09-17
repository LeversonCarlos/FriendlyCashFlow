import { NgModule } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatListModule } from '@angular/material/list';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu'
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';

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
import { LocaleService, LocaleServiceProvider } from './locale/locale.service';
import { AppSettingsService } from './app-settings/app-settings.service';
import { AppInsightsService } from './app-insights/app-insights.service';
import { AppVersionComponent } from './app-version/app-version.component';
import { LazyLoadingComponent } from './lazy-loading/lazy-loading.component';

@NgModule({
   declarations: [BaseLayoutComponent, FullLayoutComponent, SideNavComponent, AppVersionComponent,
      BusyComponent, TranslationPipe, MessageComponent, ConfirmComponent, FabButtonComponent, RelatedBoxComponent, LazyLoadingComponent],
   imports: [
      CommonModule, RouterModule, FormsModule, ReactiveFormsModule,
      MatToolbarModule, MatSidenavModule, MatCardModule, MatTabsModule, MatListModule, MatExpansionModule,
      MatIconModule, MatBadgeModule, MatButtonModule, MatMenuModule, MatProgressBarModule,
      MatFormFieldModule, MatInputModule, MatAutocompleteModule, MatSelectModule, MatSlideToggleModule,
      MatDatepickerModule, MatMomentDateModule,
      MatSnackBarModule, MatDialogModule, MatTooltipModule
   ],
   exports: [
      RouterModule, FormsModule, ReactiveFormsModule,
      MatToolbarModule, MatSidenavModule, MatCardModule, MatTabsModule, MatListModule, MatExpansionModule,
      MatIconModule, MatBadgeModule, MatButtonModule, MatMenuModule, MatProgressBarModule,
      MatFormFieldModule, MatInputModule, MatAutocompleteModule, MatSelectModule, MatSlideToggleModule,
      MatDatepickerModule, MatMomentDateModule,
      MatTooltipModule,
      BaseLayoutComponent, FullLayoutComponent, AppVersionComponent,
      FabButtonComponent, RelatedBoxComponent,
      TranslationPipe
   ],
   providers: [BusyService, TranslationService, MessageService, LocaleService, LocaleServiceProvider,
      AppSettingsService, AppInsightsService, DecimalPipe]
})
export class SharedModule { }
