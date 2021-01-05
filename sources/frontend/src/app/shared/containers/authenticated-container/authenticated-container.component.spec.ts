import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { AuthenticatedContainerComponent } from './authenticated-container.component';

describe('AuthenticatedContainerComponent', () => {

   let component: AuthenticatedContainerComponent;
   let fixture: ComponentFixture<AuthenticatedContainerComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [AuthenticatedContainerComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(AuthenticatedContainerComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
