import { Injectable, Injector } from '@angular/core';
import { AccountsData } from '@elesse/accounts';
import { CategoriesData } from '@elesse/categories';
import { EntriesData } from '@elesse/entries';
import { TokenService } from '@elesse/identity';
import { PatternsData } from '@elesse/patterns';

@Injectable({
   providedIn: 'root'
})
export class MainData {

   constructor(private injector: Injector) { }

   public RefreshAll() {
      if (!this.injector.get<TokenService>(TokenService).HasToken)
         return;
      console.log("RefreshAll")
      // this.injector.get<AccountsData>(AccountsData).RefreshAccounts();
      // this.injector.get<CategoriesData>(CategoriesData).RefreshCategories();
      // this.injector.get<EntriesData>(EntriesData).RefreshEntries();
      this.injector.get<PatternsData>(PatternsData).RefreshPatterns();
   }

}
