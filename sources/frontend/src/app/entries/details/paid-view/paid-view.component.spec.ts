import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { PaidViewComponent } from './paid-view.component';

describe('PaidViewComponent', () => {
   let component: PaidViewComponent;
   let fixture: ComponentFixture<PaidViewComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [PaidViewComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(PaidViewComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
