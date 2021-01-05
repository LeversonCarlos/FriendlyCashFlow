import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { MaterialModule } from '@elesse/material';
import { SharedModule } from '@elesse/shared';

import { RegisterComponent } from './register.component';

describe('RegisterComponent', () => {

   let component: RegisterComponent;
   let fixture: ComponentFixture<RegisterComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [RegisterComponent],
         imports: [
            RouterTestingModule, HttpClientTestingModule, MaterialModule, SharedModule, NoopAnimationsModule
         ]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(RegisterComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });
});
