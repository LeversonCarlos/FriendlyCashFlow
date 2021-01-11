import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { ConfirmViewComponent } from './confirm-view.component';

describe('ConfirmViewComponent', () => {
   let component: ConfirmViewComponent;
   let fixture: ComponentFixture<ConfirmViewComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [ConfirmViewComponent],
         imports: [TestsModule]
      })
         .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(ConfirmViewComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
