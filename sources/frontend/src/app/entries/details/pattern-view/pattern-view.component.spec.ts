import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PatternsData } from '@elesse/patterns';
import { TestsModule } from '@elesse/tests';
import { PatternViewComponent } from './pattern-view.component';

describe('PatternViewComponent', () => {
   let component: PatternViewComponent;
   let fixture: ComponentFixture<PatternViewComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [PatternViewComponent],
         imports: [TestsModule],
         providers: [PatternsData]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(PatternViewComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
