import { Account } from '../accounts/accounts.service';
import { Category, enCategoryType } from '../categories/categories.service';

export class Entry {
   EntryID: number;
   Type: enCategoryType;
   Text: string;

   PatternID: number;

   CategoryID: number;
   CategoryRow?: Category;
   CategoryText: string;

   DueDate: Date;
   EntryValue: number;

   Paid: boolean;
   PayDate?: Date;
   Delayed: boolean

   AccountID?: number;
   AccountRow?: Account;
   AccountText: string;

   RecurrencyID?: number;
   Recurrency?: any;

   TransferID: string;

   BalanceTotalValue: number
   BalancePaidValue: number
   Sorting: number
}

export class EntryFlow {
   Day: string
   EntryList: Entry[]
   BalanceTotalValue: number
   BalancePaidValue: number
}

export enum enCurrentListType { Flow = 0, Search = 1 }
export class CurrentData {

   public ListType: enCurrentListType

   public CurrentMonth: Date
   public CurrentAccount: number
   public CurrentSearchText: string

   public setFlow(currentMonth: Date, currentAccount?: number) {
      this.CurrentMonth = currentMonth
      this.CurrentAccount = currentAccount
      this.CurrentSearchText = null
      this.ListType = enCurrentListType.Flow
   }

   public setSearch(currentSearchText: string, currentAccount?: number) {
      this.CurrentMonth = null
      this.CurrentAccount = currentAccount
      this.CurrentSearchText = currentSearchText
      this.ListType = enCurrentListType.Search
   }

}
