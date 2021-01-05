import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { MaterialModule } from '@elesse/material';
import { SharedModule } from '@elesse/shared';
import { LoginComponent } from './login.component';

describe('LoginComponent', () => {

   let component: LoginComponent;
   let fixture: ComponentFixture<LoginComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [LoginComponent],
         imports: [
            RouterTestingModule, HttpClientTestingModule, MaterialModule, SharedModule, NoopAnimationsModule
         ]
      })
         .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(LoginComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
