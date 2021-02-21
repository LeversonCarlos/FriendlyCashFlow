import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { RemoveComponent } from './remove.component';

describe('RemoveComponent', () => {
   let component: RemoveComponent;
   let fixture: ComponentFixture<RemoveComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [RemoveComponent],
         imports: [TestsModule]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(RemoveComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
