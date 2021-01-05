import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { SharedModule } from '@elesse/shared';
import { LogoutComponent } from './logout.component';

describe('LogoutComponent', () => {

   let component: LogoutComponent;
   let fixture: ComponentFixture<LogoutComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [LogoutComponent],
         imports: [
            RouterTestingModule, SharedModule
         ]
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
