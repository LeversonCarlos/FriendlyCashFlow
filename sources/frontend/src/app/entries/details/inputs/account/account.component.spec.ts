import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { AccountComponent } from './account.component';

describe('AccountComponent', () => {
   let component: AccountComponent;
   let fixture: ComponentFixture<AccountComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [AccountComponent],
         imports: [TestsModule]
      })
         .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(AccountComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
