import { Injectable } from '@angular/core';
import { AccountsData } from '@elesse/accounts';
import { CategoriesData } from '@elesse/categories';
import { EntriesData } from '@elesse/entries';

@Injectable({
   providedIn: 'root'
})
export class MainData {

   constructor(private accounts: AccountsData, categories: CategoriesData, entries: EntriesData) { }

}
