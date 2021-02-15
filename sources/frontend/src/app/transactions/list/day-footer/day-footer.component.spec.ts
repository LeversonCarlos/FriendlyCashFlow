import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { DayFooterComponent } from './day-footer.component';

describe('DayFooterComponent', () => {
   let component: DayFooterComponent;
   let fixture: ComponentFixture<DayFooterComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [DayFooterComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(DayFooterComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
