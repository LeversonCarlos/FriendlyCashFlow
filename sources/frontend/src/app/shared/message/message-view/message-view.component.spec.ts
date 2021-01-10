import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatSnackBarRef, MAT_SNACK_BAR_DATA } from '@elesse/material';
import { TestsModule } from '@elesse/tests';
import { MessageViewComponent } from './message-view.component';

describe('MessageViewComponent', () => {

   let component: MessageViewComponent;
   let fixture: ComponentFixture<MessageViewComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [MessageViewComponent],
         imports: [TestsModule],
         providers: [{
            provide: MatSnackBarRef,
            useValue: {}
         },
         {
            provide: MAT_SNACK_BAR_DATA,
            useValue: {} // Add any data you wish to test if it is passed/used correctly
         }]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(MessageViewComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
