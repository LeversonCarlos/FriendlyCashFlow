import { TestBed } from '@angular/core/testing';
import { AccountEntity } from './accounts.data';
import { AccountsService } from './accounts.service';

describe('AccountsService', () => {

   let service: AccountsService;

   beforeEach(() => {
      TestBed.configureTestingModule({});
      service = TestBed.inject(AccountsService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

   it('GetData WhenDataIsNull MustResult EmptyArray', () => {
      const account = Object.assign(new AccountEntity, { Text: 'Account Text' });

      service.GetData(false).subscribe(result => {
         expect(result).toEqual([account]);
         expect(result[0].Text).toEqual(account.Text);
         // done();
      });

      service.SetData(false, [account])
   });

});
