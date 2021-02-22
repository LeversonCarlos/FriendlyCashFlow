import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { IAccountSelectorService } from './account-selector.interface';
import { AccountSelectorService } from './account-selector.service';

describe('AccountSelectorService', () => {

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });

   });

   it('should be created', () => {
      const service = TestBed.inject(AccountSelectorService);
      expect(service).toBeTruthy();
   });

   it('Provider must result instance of AccountSelectorService', () => {
      const s = TestBed.inject(IAccountSelectorService);
      s.SetTab(1, '1')
      expect(s.TabIndex).toEqual(1);
      expect(s.AccountID).toEqual('1');
      expect(s).toBeInstanceOf(AccountSelectorService);
   });

});
