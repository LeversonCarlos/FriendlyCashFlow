import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';

import { CategoryViewComponent } from './category-view.component';

describe('CategoryViewComponent', () => {
   let component: CategoryViewComponent;
   let fixture: ComponentFixture<CategoryViewComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [CategoryViewComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(CategoryViewComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
