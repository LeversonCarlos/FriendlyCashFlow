import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { PaidComponent } from './paid.component';

describe('PaidComponent', () => {
   let component: PaidComponent;
   let fixture: ComponentFixture<PaidComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [PaidComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(PaidComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
