import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { DetailsRouteViewComponent } from './route-view.component';

describe('DetailsRouteViewComponent', () => {

   let component: DetailsRouteViewComponent;
   let fixture: ComponentFixture<DetailsRouteViewComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [DetailsRouteViewComponent],
         imports: [TestsModule]
      })
         .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(DetailsRouteViewComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
