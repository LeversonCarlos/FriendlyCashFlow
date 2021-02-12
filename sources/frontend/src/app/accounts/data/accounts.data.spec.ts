import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { AccountEntity } from '../model/accounts.model';
import { AccountsData } from './accounts.data';

describe('AccountsData', () => {

   let service: AccountsData;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(AccountsData);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

   it('ObserveAccounts InitialValue MustResult Null', (done) => {
      const account = Object.assign(new AccountEntity, { Text: 'Account Text' });

      service.ObserveAccounts(false).subscribe(result => {
         expect(result).toBeNull();
         done();
      });

   });

});
