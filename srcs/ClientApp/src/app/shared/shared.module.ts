import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import 'hammerjs';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

import { BaseLayoutComponent } from './base-layout/base-layout.component';
import { FullLayoutComponent } from './full-layout/full-layout.component';
import { SideNavComponent } from './side-nav/side-nav.component';

@NgModule({
   declarations: [BaseLayoutComponent, FullLayoutComponent, SideNavComponent],
   imports: [
      CommonModule, RouterModule,
      MatToolbarModule, MatSidenavModule, MatCardModule, MatListModule,
      MatIconModule, MatButtonModule
   ],
   exports: [
      MatToolbarModule, MatSidenavModule, MatCardModule, MatListModule,
      MatIconModule, MatButtonModule,
      BaseLayoutComponent, FullLayoutComponent
   ]
})
export class SharedModule { }
