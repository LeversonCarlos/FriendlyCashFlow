import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { DueDateComponent } from './due-date.component';

describe('DueDateComponent', () => {
   let component: DueDateComponent;
   let fixture: ComponentFixture<DueDateComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [DueDateComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(DueDateComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
