import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { EmptyListComponent } from './empty-list.component';

describe('EmptyListComponent', () => {

   let component: EmptyListComponent;
   let fixture: ComponentFixture<EmptyListComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [EmptyListComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(EmptyListComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });
});
