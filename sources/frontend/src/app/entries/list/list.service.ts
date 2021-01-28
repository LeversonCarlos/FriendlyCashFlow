import { Injectable } from '@angular/core';
import { AccountEntries, EntryEntity } from '../entries.data';

@Injectable({
   providedIn: 'root'
})
export class ConvertService {

   constructor() { }

   public GetEntriesAccounts(entries: EntryEntity[]): AccountEntries[] {
      if (entries == null || entries.length == 0)
         return [];
   }

}
