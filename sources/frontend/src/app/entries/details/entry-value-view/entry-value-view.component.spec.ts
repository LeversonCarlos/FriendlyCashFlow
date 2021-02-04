import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { EntryValueViewComponent } from './entry-value-view.component';

describe('EntryValueViewComponent', () => {
   let component: EntryValueViewComponent;
   let fixture: ComponentFixture<EntryValueViewComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [EntryValueViewComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(EntryValueViewComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
