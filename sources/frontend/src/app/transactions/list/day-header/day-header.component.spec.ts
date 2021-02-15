import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { DayHeaderComponent } from './day-header.component';

describe('DayHeaderComponent', () => {
   let component: DayHeaderComponent;
   let fixture: ComponentFixture<DayHeaderComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [DayHeaderComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(DayHeaderComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
