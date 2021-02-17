import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';

import { IncomeAccountComponent } from './income-account.component';

describe('IncomeAccountComponent', () => {
   let component: IncomeAccountComponent;
   let fixture: ComponentFixture<IncomeAccountComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [IncomeAccountComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(IncomeAccountComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
