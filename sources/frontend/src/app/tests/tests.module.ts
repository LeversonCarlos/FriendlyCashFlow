import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { SharedModule } from '@elesse/shared';
import { MaterialModule } from '@elesse/material';

@NgModule({
   declarations: [],
   imports: [
      CommonModule,
      RouterTestingModule, HttpClientTestingModule,
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
