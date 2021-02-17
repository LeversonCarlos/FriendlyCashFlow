import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';

import { DateComponent } from './date.component';

describe('DateComponent', () => {
   let component: DateComponent;
   let fixture: ComponentFixture<DateComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [DateComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(DateComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
