import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SharedModule } from '@elesse/shared';
import { MaterialModule } from '@elesse/material';
import { BlankComponent } from './blank.component';

const dummyRoutes = [
   { path: 'accounts', component: BlankComponent },
   { path: 'accounts/list', component: BlankComponent },
   { path: 'accounts/details/:id', component: BlankComponent },
   { path: 'categories', component: BlankComponent },
   { path: 'categories/list', component: BlankComponent },
   { path: 'categories/details/:id', component: BlankComponent },
   { path: 'transactions', component: BlankComponent },
   { path: 'transactions/list', component: BlankComponent },
   { path: 'entries', component: BlankComponent },
   { path: 'entries/details/:id', component: BlankComponent },
];

@NgModule({
   declarations: [BlankComponent],
   imports: [
      CommonModule,
      RouterTestingModule.withRoutes(dummyRoutes), HttpClientTestingModule,
      SharedModule, MaterialModule,
      NoopAnimationsModule,
   ],
   exports: [
      RouterTestingModule, HttpClientTestingModule,
      SharedModule, MaterialModule,
      NoopAnimationsModule,
   ]
})
export class TestsModule { }
