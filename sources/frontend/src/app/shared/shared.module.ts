import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { BusyComponent } from './busy/busy.component';
import { AnonymousContainerComponent } from './containers/anonymous-container/anonymous-container.component';
import { AuthenticatedContainerComponent } from './containers/authenticated-container/authenticated-container.component';
import { MaterialModule } from '@elesse/material';

@NgModule({
   declarations: [
      BusyComponent,
      AnonymousContainerComponent, AuthenticatedContainerComponent
   ],
   imports: [
      CommonModule, MaterialModule, FormsModule, ReactiveFormsModule
   ],
   exports: [
      CommonModule, FormsModule, ReactiveFormsModule,
      BusyComponent,
      AnonymousContainerComponent, AuthenticatedContainerComponent
   ]
})
export class SharedModule { }
