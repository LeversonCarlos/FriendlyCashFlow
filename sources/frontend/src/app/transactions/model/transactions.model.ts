import { AccountEntity } from "@elesse/accounts";
import { enCategoryType } from "@elesse/categories";
import { EntryEntity } from "@elesse/entries";
import { TransferEntity } from "@elesse/transfers";

export class TransactionAccount {
   Account: AccountEntity;
   AccountIcon: string;
   Balance: Balance = new Balance();
   Days: TransactionDay[] = []
   public static Parse(value: AccountEntity): TransactionAccount {
      return Object.assign(new TransactionAccount, {
         Account: value
      });
   }
}

export class TransactionDay {
   Day: Date;
   Balance: Balance = new Balance();
   Transactions: TransactionBase[] = []
}

export class Balance {
   Expected: number = 0.00;
   Realized: number = 0.00;
}

export abstract class TransactionBase {
   Date: Date;
   Text: string
   Details: string
   Value: number;
   Paid: boolean;
   Sorting: number;
   Balance: Balance = new Balance();
}

export class TransactionEntry extends TransactionBase {
   Entry: EntryEntity;
   static Parse(value: EntryEntity): TransactionEntry {
      if (!value)
         return null;
      return Object.assign(new TransactionEntry, {
         Date: new Date(value.Paid && value.PayDate ? value.PayDate : value.DueDate),
         Text: value.Pattern?.Text ?? '',
         Value: value.Value * ((value.Pattern?.Type ?? enCategoryType.Income) == enCategoryType.Income ? 1 : -1),
         Paid: value.Paid,
         Sorting: value.Sorting,
         Entry: value
      });
   }
}

export class TransactionTransfer extends TransactionBase {
   Transfer: TransferEntity;
   static Parse(value: TransferEntity, text: string, type: enCategoryType): TransactionTransfer {
      if (!value)
         return null;
      return Object.assign(new TransactionTransfer, {
         Date: new Date(value.Date),
         Text: text,
         Value: value.Value * ((type ?? enCategoryType.Income) == enCategoryType.Income ? 1 : -1),
         Paid: true,
         Sorting: value.Sorting,
         Transfer: value
      });
   }
}

export class TransactionBalance extends TransactionBase {
   static Parse(date: Date, text: string, value: number, sorting: number): TransactionBalance {
      return Object.assign(new TransactionBalance, {
         Date: new Date(date),
         Text: text,
         Value: value,
         Paid: true,
         Sorting: sorting
      });
   }
}
