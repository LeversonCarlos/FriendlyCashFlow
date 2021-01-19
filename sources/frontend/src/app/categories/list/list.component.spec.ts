import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { ListBodyComponent } from './list-body/list-body.component';
import { ListComponent } from './list.component';

describe('ListComponent', () => {

   let component: ListComponent;
   let fixture: ComponentFixture<ListComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [ListComponent, ListBodyComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(ListComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
