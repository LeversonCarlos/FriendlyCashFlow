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
