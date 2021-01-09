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
      expect(service.GetData()).toEqual([]);
      expect(service.GetData(false)).toEqual([]);
   });

   it('GetData WhenDataIsNull MustResult EmptyArray', () => {
      const account = Object.assign(new AccountEntity, { Text: 'Account Text' });

      service.SetData(false, [account])
      const result = service.GetData(false);

      expect(result).toEqual([account]);
      expect(result[0].Text).toEqual(account.Text);
   });

});
