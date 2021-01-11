import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { MatDialogRef, MAT_DIALOG_DATA } from '@elesse/material';
import { ConfirmViewComponent } from './confirm-view.component';

describe('ConfirmViewComponent', () => {
   let component: ConfirmViewComponent;
   let fixture: ComponentFixture<ConfirmViewComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [ConfirmViewComponent],
         imports: [TestsModule],
         providers: [{
            provide: MatDialogRef,
            useValue: {}
         },
         {
            provide: MAT_DIALOG_DATA,
            useValue: {} // Add any data you wish to test if it is passed/used correctly
         }]
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
