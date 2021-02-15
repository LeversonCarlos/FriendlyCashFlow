import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { ItemComponent } from './item.component';

describe('ItemComponent', () => {
   let component: ItemComponent;
   let fixture: ComponentFixture<ItemComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [ItemComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(ItemComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
