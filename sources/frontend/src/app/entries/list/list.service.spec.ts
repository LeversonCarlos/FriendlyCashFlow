import { AccountEntity } from '@elesse/accounts';
import { EntryEntity } from '../entries.data';
import { ListService } from './list.service';

describe('ListService', () => {

   it('GetEntriesAccounts with null parameters shold result empty array', () => {
      expect(ListService.GetEntriesAccounts(null, null)).toEqual([])
   });

   it('GetEntriesAccounts with empty parameters should result empty array', () => {
      const accounts: AccountEntity[] = []
      const entries: EntryEntity[] = []
      expect(ListService.GetEntriesAccounts(accounts, entries)).toEqual([])
   });

   it('GetEntriesDays with null parameter shold result empty array', () => {
      const entries: EntryEntity[] = null
      expect(ListService.GetEntriesDays(entries)).toEqual([])
   });

   it('GetEntriesDays with empty parameter should result empty array', () => {
      const entries: EntryEntity[] = []
      expect(ListService.GetEntriesDays(entries)).toEqual([])
   });

});
