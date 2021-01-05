import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { MaterialModule } from '@elesse/material';
import { SharedModule } from '@elesse/shared';
import { ChangePasswordComponent } from './change-password.component';

describe('ChangePasswordComponent', () => {

   let component: ChangePasswordComponent;
   let fixture: ComponentFixture<ChangePasswordComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [ChangePasswordComponent],
         imports: [RouterTestingModule, HttpClientTestingModule, SharedModule, MaterialModule, NoopAnimationsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(ChangePasswordComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
