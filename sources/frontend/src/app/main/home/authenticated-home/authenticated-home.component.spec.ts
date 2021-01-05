import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { SharedModule } from '@elesse/shared';
import { AuthenticatedHomeComponent } from './authenticated-home.component';

describe('AuthenticatedHomeComponent', () => {

   let component: AuthenticatedHomeComponent;
   let fixture: ComponentFixture<AuthenticatedHomeComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [AuthenticatedHomeComponent],
         imports: [SharedModule, RouterTestingModule, NoopAnimationsModule]
      })
         .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(AuthenticatedHomeComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
