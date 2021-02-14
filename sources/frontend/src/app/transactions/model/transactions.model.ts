import { AccountEntity } from "@elesse/accounts";
import { EntryEntity } from "@elesse/entries";

export class TransactionAccount {
   Account: AccountEntity;
   Balance: Balance;
   Days: TransactionDay[] = []
}

export class TransactionDay {
   Day: Date;
   Balance: Balance
   Transactions: TransactionBase[] = []
}

export class Balance {
   Expected: number = 0.00;
   Realized: number = 0.00;
}

export abstract class TransactionBase {
   Text: string
   Date: Date;
   Value: number;
   Paid: boolean;
   Balance: Balance;
}

export class TransactionEntry extends TransactionBase {
   Entry: EntryEntity;
}
