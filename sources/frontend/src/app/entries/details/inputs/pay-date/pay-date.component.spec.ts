import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { PayDateComponent } from './pay-date.component';

describe('PayDateComponent', () => {
   let component: PayDateComponent;
   let fixture: ComponentFixture<PayDateComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [PayDateComponent],
         imports: [TestsModule]
      })
         .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(PayDateComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
