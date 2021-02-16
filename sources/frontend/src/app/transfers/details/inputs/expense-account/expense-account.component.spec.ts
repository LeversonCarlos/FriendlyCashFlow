import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { ExpenseAccountComponent } from './expense-account.component';

describe('ExpenseAccountComponent', () => {
   let component: ExpenseAccountComponent;
   let fixture: ComponentFixture<ExpenseAccountComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [ExpenseAccountComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(ExpenseAccountComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
