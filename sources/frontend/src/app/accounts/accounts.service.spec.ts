import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { AccountEntity } from './accounts.data';
import { AccountsService } from './accounts.service';

describe('AccountsService', () => {

   let service: AccountsService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(AccountsService);
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
