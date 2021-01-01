import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { RootComponent } from './root.component';

describe('RootComponent', () => {

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         imports: [
            RouterTestingModule
         ],
         declarations: [
            RootComponent
         ],
      }).compileComponents();
   });

   it('should create the app', () => {
      const fixture = TestBed.createComponent(RootComponent);
      const app = fixture.componentInstance;
      expect(app).toBeTruthy();
   });

   it(`should have as title 'CashFlow'`, () => {
      const fixture = TestBed.createComponent(RootComponent);
      const app = fixture.componentInstance;
      expect(app.title).toEqual('CashFlow');
   });

   it('should render title', () => {
      const fixture = TestBed.createComponent(RootComponent);
      fixture.detectChanges();
      const compiled = fixture.nativeElement;
      expect(compiled.querySelector('.content span').textContent).toContain('CashFlow app is running!');
   });

});
