import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AccountsData } from '@elesse/accounts';
import { TestsModule } from '@elesse/tests';
import { AccountViewComponent } from './account-view.component';

describe('AccountViewComponent', () => {
   let component: AccountViewComponent;
   let fixture: ComponentFixture<AccountViewComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         declarations: [AccountViewComponent],
         imports: [TestsModule],
         providers: [AccountsData]
      })
      .compileComponents();
   });

   beforeEach(() => {
      fixture = TestBed.createComponent(AccountViewComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });

});
