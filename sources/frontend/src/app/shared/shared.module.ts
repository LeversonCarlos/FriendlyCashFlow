import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { BusyComponent } from './busy/busy.component';
import { AnonymousContainerComponent } from './containers/anonymous-container/anonymous-container.component';
import { AuthenticatedContainerComponent } from './containers/authenticated-container/authenticated-container.component';
import { MaterialModule } from '@elesse/material';
import { LinksComponent } from './containers/links/links.component';

@NgModule({
   declarations: [
      BusyComponent,
      AnonymousContainerComponent, AuthenticatedContainerComponent, LinksComponent
   ],
   imports: [
      CommonModule, RouterModule, FormsModule, ReactiveFormsModule,
      MaterialModule
   ],
   exports: [
      CommonModule, RouterModule, FormsModule, ReactiveFormsModule,
      BusyComponent,
      AnonymousContainerComponent, AuthenticatedContainerComponent
   ]
})
export class SharedModule { }
