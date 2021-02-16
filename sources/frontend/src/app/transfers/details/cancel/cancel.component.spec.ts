import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';

import { CancelComponent } from './cancel.component';

describe('CancelComponent', () => {
   let component: CancelComponent;
   let fixture: ComponentFixture<CancelComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [CancelComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(CancelComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
