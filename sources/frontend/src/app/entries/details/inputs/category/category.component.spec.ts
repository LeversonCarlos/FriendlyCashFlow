import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CategoriesData } from '@elesse/categories';
import { TestsModule } from '@elesse/tests';
import { CategoryComponent } from './category.component';

describe('CategoryComponent', () => {
   let component: CategoryComponent;
   let fixture: ComponentFixture<CategoryComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [CategoryComponent],
         imports: [TestsModule],
         providers: [CategoriesData]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(CategoryComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
