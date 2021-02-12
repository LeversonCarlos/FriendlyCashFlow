import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { RegisterComponent } from './register.component';

describe('RegisterComponent', () => {

   let component: RegisterComponent;
   let fixture: ComponentFixture<RegisterComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [RegisterComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(RegisterComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });
});
