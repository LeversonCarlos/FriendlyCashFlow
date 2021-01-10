import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';

import { LinksComponent } from './links.component';

describe('LinksComponent', () => {

   let component: LinksComponent;
   let fixture: ComponentFixture<LinksComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [LinksComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(LinksComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
