import { AccountEntity, AccountsData } from "@elesse/accounts";
import { CategoryEntity, enCategoryType } from "@elesse/categories";
import { EntryEntity } from "@elesse/entries";
import { TransferEntity } from "@elesse/transfers";
import { combineLatest, Observable } from "rxjs";
import { map } from "rxjs/operators";
import { Balance, TransactionAccount, TransactionBase, TransactionDay, TransactionEntry, TransactionTransfer } from "../model/transactions.model";

export class TransactionsConverter {

   public static Merge = (
      accountsObservable: Observable<AccountEntity[]>,
      incomeCategoriesObservable: Observable<CategoryEntity[]>,
      expenseCategoriesObservable: Observable<CategoryEntity[]>,
      entriesObservable: Observable<EntryEntity[]>, transfersObservable: Observable<TransferEntity[]>
   ): Observable<TransactionAccount[]> =>
      combineLatest([accountsObservable, incomeCategoriesObservable, expenseCategoriesObservable, entriesObservable, transfersObservable])
         .pipe(
            map(([accounts, incomeCategories, expenseCategories, entries, transfers]) => TransactionsConverter.Convert(accounts, incomeCategories, expenseCategories, entries, transfers))
         );

   public static Convert(
      accountsParam: AccountEntity[],
      incomeCategoriesList: CategoryEntity[],
      expenseCategoriesList: CategoryEntity[],
      entriesParam: EntryEntity[],
      transfersParam: TransferEntity[]
   ): TransactionAccount[] {
      if (accountsParam == null || accountsParam.length == 0)
         return [];

      const accountsDict = TransactionsConverter.GetAccountsDictionary(accountsParam);
      TransactionsConverter.DistributeEntriesThroughAccounts(accountsDict, incomeCategoriesList, expenseCategoriesList, entriesParam);
      TransactionsConverter.DistributeTransfersThroughAccounts(accountsDict, transfersParam);
      const accountsResult = TransactionsConverter.ParseDictionaryDataIntoTransactionData(accountsDict, accountsParam);

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

   private static DistributeEntriesThroughAccounts(
      accountsDict: Record<string, AccountDict>,
      incomeCategoriesList: CategoryEntity[],
      expenseCategoriesList: CategoryEntity[],
      entriesParam: EntryEntity[]) {

      // LOOP THROUGH THE ENTRIES LIST
      if (entriesParam?.length > 0) {
         for (let entryIndex = 0; entryIndex < entriesParam.length; entryIndex++) {

            // PARSE ENTRY INTO TRANSATION
            const transaction = TransactionEntry.Parse(entriesParam[entryIndex]);
            transaction.Details = TransactionsConverter.GetCategoryDescription(transaction.Entry, incomeCategoriesList, expenseCategoriesList);

            // LOCATE ACCOUNT ON DICTIONARY
            const accountDict = accountsDict[transaction.Entry.AccountID];

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

   private static DistributeTransfersThroughAccounts(
      accountsDict: Record<string, AccountDict>,
      transfersParam: TransferEntity[]) {

      // LOOP THROUGH THE ENTRIES LIST
      if (transfersParam?.length > 0) {
         for (let transferIndex = 0; transferIndex < transfersParam.length; transferIndex++) {
            const transfer = transfersParam[transferIndex];

            // PREPARE BOTH ACCOUNTS CASE
            const transferAccounts = [
               { AccountID: transfer.ExpenseAccountID, Type: enCategoryType.Expense },
               { AccountID: transfer.IncomeAccountID, Type: enCategoryType.Income }
            ];
            for (let transferAccountIndex = 0; transferAccountIndex < transferAccounts.length; transferAccountIndex++) {
               const transferAccount = transferAccounts[transferAccountIndex];

               // LOCATE ACCOUNT ON DICTIONARY
               const accountDict = accountsDict[transferAccount.AccountID];

               // PARSE ENTRY INTO TRANSATION
               const transaction = TransactionTransfer.Parse(transfer, accountDict.Account.Text, transferAccount.Type);

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

   private static GetCategoryDescription(entry: EntryEntity,
      incomeCategoriesList: CategoryEntity[],
      expenseCategoriesList: CategoryEntity[]) {

      if (entry?.Pattern?.CategoryID && entry?.Pattern?.Type) {
         if (entry?.Pattern?.Type == enCategoryType.Income && incomeCategoriesList) {
            return incomeCategoriesList
               .filter(cat => cat.CategoryID == entry.Pattern.CategoryID)
               .map(cat => cat.HierarchyText)
               .reduce((acc, cur) => cur, '');
         }
         else if (entry?.Pattern?.Type == enCategoryType.Expense && expenseCategoriesList) {
            return expenseCategoriesList
               .filter(cat => cat.CategoryID == entry.Pattern.CategoryID)
               .map(cat => cat.HierarchyText)
               .reduce((acc, cur) => cur, '');
         }
      }
   }

   private static ParseDictionaryDataIntoTransactionData(accountsDict: Record<string, AccountDict>, accountsParam: AccountEntity[]): TransactionAccount[] {
      let accountsResult: TransactionAccount[] = [];

      // LOOP THROUGH ALREADY SORTED ACCOUNTS
      for (let accountIndex = 0; accountIndex < accountsParam.length; accountIndex++) {

         // PARSE ACCOUNT INTO TRANSACTION
         const transactionAccount = TransactionAccount.Parse(accountsParam[accountIndex]);
         transactionAccount.AccountIcon = AccountsData.GetAccountIcon(transactionAccount.Account.Type);

         // RETRIEVE ACCOUNT FROM DICTIONARY
         const accountDict = accountsDict[transactionAccount.Account.AccountID];

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

      return accountsResult;
   }

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
