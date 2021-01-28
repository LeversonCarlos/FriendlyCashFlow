import { EntryEntity } from '../entries.data';
import { ListService } from './list.service';

describe('ListService', () => {

   it('GetEntriesAccounts with null parameter shold result empty array', () => {
      const entries: EntryEntity[] = null
      expect(ListService.GetEntriesAccounts(entries)).toEqual([])
   });

   it('GetEntriesAccounts with empty parameter should result empty array', () => {
      const entries: EntryEntity[] = []
      expect(ListService.GetEntriesAccounts(entries)).toEqual([])
   });

});
