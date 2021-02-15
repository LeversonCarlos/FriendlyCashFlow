import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { DaysComponent } from './days.component';

describe('DaysComponent', () => {
   let component: DaysComponent;
   let fixture: ComponentFixture<DaysComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [DaysComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(DaysComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
