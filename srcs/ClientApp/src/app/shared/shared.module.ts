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
import { MatSlideToggleModule } from '@angular/material/slide-toggle';

import { BaseLayoutComponent } from './base-layout/base-layout.component';
import { FullLayoutComponent } from './full-layout/full-layout.component';
import { SideNavComponent } from './side-nav/side-nav.component';
import { BusyComponent } from './busy/busy.component';
import { BusyService } from './busy/busy.service';

@NgModule({
   declarations: [BaseLayoutComponent, FullLayoutComponent, SideNavComponent, BusyComponent],
   imports: [
      CommonModule, RouterModule, FormsModule, ReactiveFormsModule,
      MatToolbarModule, MatSidenavModule, MatCardModule, MatTabsModule, MatListModule,
      MatIconModule, MatButtonModule, MatProgressBarModule,
      MatFormFieldModule, MatInputModule, MatSlideToggleModule
   ],
   exports: [
      FormsModule, ReactiveFormsModule,
      MatToolbarModule, MatSidenavModule, MatCardModule, MatTabsModule, MatListModule,
      MatIconModule, MatButtonModule, MatProgressBarModule,
      MatFormFieldModule, MatInputModule, MatSlideToggleModule,
      BaseLayoutComponent, FullLayoutComponent
   ],
   providers: [BusyService]
})
export class SharedModule { }
