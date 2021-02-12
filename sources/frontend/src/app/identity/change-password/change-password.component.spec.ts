import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { ChangePasswordComponent } from './change-password.component';

describe('ChangePasswordComponent', () => {

   let component: ChangePasswordComponent;
   let fixture: ComponentFixture<ChangePasswordComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [ChangePasswordComponent],
         imports: [TestsModule]
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
