import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { SharedModule } from '@elesse/shared';
import { AnonymousHomeComponent } from './anonymous-home.component';

describe('AnonymousHomeComponent', () => {

   let component: AnonymousHomeComponent;
   let fixture: ComponentFixture<AnonymousHomeComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [AnonymousHomeComponent],
         imports: [SharedModule, RouterTestingModule, NoopAnimationsModule]
      })
         .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(AnonymousHomeComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
