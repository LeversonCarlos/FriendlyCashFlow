import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { RootComponent } from './root.component';
import { SharedModule } from '../shared/shared.module';

describe('RootComponent', () => {

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         imports: [
            RouterTestingModule, HttpClientTestingModule, SharedModule,
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

});
