import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { LogoutComponent } from './logout.component';

describe('LogoutComponent', () => {

   let component: LogoutComponent;
   let fixture: ComponentFixture<LogoutComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [LogoutComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(LogoutComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
