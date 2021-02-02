import { Injectable, Injector } from '@angular/core';
import { AccountsData } from '@elesse/accounts';
import { CategoriesData } from '@elesse/categories';
import { EntriesData } from '@elesse/entries';

@Injectable({
   providedIn: 'root'
})
export class MainData {

   constructor(private injector: Injector) { }

   public RefreshAll() {
      console.log("RefreshAll")
      this.injector.get<AccountsData>(AccountsData).RefreshAccounts();
      this.injector.get<CategoriesData>(CategoriesData).RefreshCategories();
      this.injector.get<EntriesData>(EntriesData).RefreshEntries();
   }

}
