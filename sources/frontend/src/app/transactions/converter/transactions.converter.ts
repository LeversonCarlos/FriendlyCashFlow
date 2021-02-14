import { AccountEntity } from "@elesse/accounts";
import { EntryEntity } from "@elesse/entries";
import { Balance, TransactionAccount, TransactionBase, TransactionEntry } from "../model/transactions.model";

export class TransactionsConverter {

   public static Convert(accountsParam: AccountEntity[], entriesParam: EntryEntity[]): TransactionAccount[] {
      if (accountsParam == null || accountsParam.length == 0)
         return [];

      // PREPARE THE ACCOUNT DICTIONARY
      let mainDict = new MainDict();
      for (let accountIndex = 0; accountIndex < accountsParam.length; accountIndex++) {
         const account = accountsParam[accountIndex];
         mainDict[account.AccountID] = Object.assign(new AccountDict, { Account: account });
      }

      // LOOP THROUGH THE ENTRIES LIST
      if (entriesParam?.length > 0) {
         for (let entryIndex = 0; entryIndex < entriesParam.length; entryIndex++) {

            // PARSE ENTRY INTO TRANSATION
            const entry = entriesParam[entryIndex];
            const transaction = TransactionEntry.Parse(entry);

            // LOCATE ACCOUNT ON DICTIONARY
            const accountDict = mainDict.Accounts[entry.AccountID];

            // SUM TRANSACTION VALUE INTO ACCOUNT GENERAL BALANCE
            accountDict.Balance.Expected += transaction.Value;
            if (transaction.Paid)
               accountDict.Balance.Realized += transaction.Value;

            // LOCATE DAY ON DICTIONARY
            const dayDict = accountDict.GetDay(transaction.Date);

            // SUM TRANSACTION VALUE INTO DAY BALANCE
            dayDict.Balance.Expected += transaction.Value;
            if (transaction.Paid)
               dayDict.Balance.Realized += transaction.Value;

            // PUSH TRANSACTION INTO DAY TRANSACTIONS
            dayDict.Transactions.push(transaction);

         }
      }

      // LOOP THROUGH ALREADY SORTED ACCOUNTS
      let accountsResult: TransactionAccount[] = [];
      for (let accountIndex = 0; accountIndex < accountsParam.length; accountIndex++) {

         // PARSE ACCOUNT INTO TRANSACTION
         const account = accountsParam[accountIndex];
         const transactionAccount = TransactionAccount.Parse(account);

         // PUSH ACCOUNT INTO RESULT
         accountsResult.push(transactionAccount);
      }

      // MAIN RESULT
      return accountsResult;
   }

}

class MainDict {
   Accounts: Record<string, AccountDict> = {};
}

class AccountDict {
   Account: AccountEntity;
   Balance: Balance = new Balance();
   Days: Record<number, DayDict> = {};

   GetDay(date: Date) {
      const day = date.getDate();
      if (this.Days[day] == null)
         this.Days[day] = Object.assign(new DayDict, { Day: day });
      return this.Days[day];
   }

}

class DayDict {
   Day: number;
   Balance: Balance = new Balance();
   Transactions: TransactionBase[] = [];
}
