import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PatternsData } from '@elesse/patterns';
import { TestsModule } from '@elesse/tests';
import { PatternComponent } from './pattern.component';

describe('PatternComponent', () => {
   let component: PatternComponent;
   let fixture: ComponentFixture<PatternComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [PatternComponent],
         imports: [TestsModule],
         providers: [PatternsData]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(PatternComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
