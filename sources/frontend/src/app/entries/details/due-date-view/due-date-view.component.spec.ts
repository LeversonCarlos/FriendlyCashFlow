import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { DueDateViewComponent } from './due-date-view.component';

describe('DueDateViewComponent', () => {
   let component: DueDateViewComponent;
   let fixture: ComponentFixture<DueDateViewComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [DueDateViewComponent],
         imports:[TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(DueDateViewComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
