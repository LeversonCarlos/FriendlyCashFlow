import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';

import { MonthSelectorComponent } from './month-selector.component';

describe('MonthSelectorComponent', () => {
   let component: MonthSelectorComponent;
   let fixture: ComponentFixture<MonthSelectorComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [MonthSelectorComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(MonthSelectorComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
