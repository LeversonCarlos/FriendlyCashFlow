import { AccountEntity } from "@elesse/accounts";
import { EntryEntity } from "@elesse/entries";
import { Balance, TransactionAccount, TransactionBase, TransactionDay, TransactionEntry } from "../model/transactions.model";

export class TransactionsConverter {

   public static Convert(accountsParam: AccountEntity[], entriesParam: EntryEntity[]): TransactionAccount[] {

      // WITHOUT ACCOUNTS, WE CAN DO MUCH
      if (accountsParam == null || accountsParam.length == 0)
         return [];

      // PREPARE THE ACCOUNTS DICTIONARY
      const accountsDict = TransactionsConverter.GetAccountsDictionary(accountsParam);

      // DISTRIBUTE ENTRIES THROUGH ACCOUNTS
      TransactionsConverter.DistributeEntriesThroughAccounts(accountsDict, entriesParam);

      // LOOP THROUGH ALREADY SORTED ACCOUNTS
      let accountsResult: TransactionAccount[] = [];
      for (let accountIndex = 0; accountIndex < accountsParam.length; accountIndex++) {

         // PARSE ACCOUNT INTO TRANSACTION
         const account = accountsParam[accountIndex];
         const transactionAccount = TransactionAccount.Parse(account);

         // RETRIEVE ACCOUNT FROM DICTIONARY
         const accountDict = accountsDict[account.AccountID];

         // APPLY ACCOUNT BALANCE
         transactionAccount.Balance.Expected = accountDict.Balance.Expected;
         transactionAccount.Balance.Realized = accountDict.Balance.Realized;

         // LOOP THROUGH DICTIONARY DAYS
         let accumulatedExpectedBalance = 0.00;
         let accumulatedRealizedBalance = 0.00;
         for (const day in accountDict.Days) {

            // PARSE DAY INTO TRANSACTION DAY
            const dayDict = accountDict.Days[day];
            const transactionDay = Object.assign(new TransactionDay, { Day: dayDict.Date });

            // APPLY DAY BALANCE
            accumulatedExpectedBalance += dayDict.Balance.Expected;
            accumulatedRealizedBalance += dayDict.Balance.Realized;
            transactionDay.Balance.Expected = accumulatedExpectedBalance;
            transactionDay.Balance.Realized = accumulatedRealizedBalance;

            // PUSH SORTED TRANSACTIONS INTO DAY
            transactionDay.Transactions = dayDict.Transactions
               .sort((p, n) => p.Sorting < n.Sorting ? -1 : 1);

            // PUSH DAY INTO RESULT
            transactionAccount.Days.push(transactionDay);
         }

         // PUSH ACCOUNT INTO RESULT
         accountsResult.push(transactionAccount);
      }

      // MAIN RESULT
      return accountsResult;
   }

   private static GetAccountsDictionary(accountsParam: AccountEntity[]) {
      let accountsDict: Record<string, AccountDict> = {};
      for (let accountIndex = 0; accountIndex < accountsParam.length; accountIndex++) {
         const account = accountsParam[accountIndex];
         accountsDict[account.AccountID] = Object.assign(new AccountDict, { Account: account });
      }
      return accountsDict;
   }

   private static DistributeEntriesThroughAccounts(accountsDict: Record<string, AccountDict>, entriesParam: EntryEntity[]) {

      // LOOP THROUGH THE ENTRIES LIST
      if (entriesParam?.length > 0) {
         for (let entryIndex = 0; entryIndex < entriesParam.length; entryIndex++) {

            // PARSE ENTRY INTO TRANSATION
            const entry = entriesParam[entryIndex];
            const transaction = TransactionEntry.Parse(entry);

            // LOCATE ACCOUNT ON DICTIONARY
            const accountDict = accountsDict[entry.AccountID];

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
         this.Days[day] = Object.assign(new DayDict, { Day: day, Date: date });
      return this.Days[day];
   }

}

class DayDict {
   Day: number;
   Date: Date;
   Balance: Balance = new Balance();
   Transactions: TransactionBase[] = [];
}
