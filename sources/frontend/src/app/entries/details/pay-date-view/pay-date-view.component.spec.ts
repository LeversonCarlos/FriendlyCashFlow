import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { PayDateViewComponent } from './pay-date-view.component';

describe('PayDateViewComponent', () => {
   let component: PayDateViewComponent;
   let fixture: ComponentFixture<PayDateViewComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [PayDateViewComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(PayDateViewComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
