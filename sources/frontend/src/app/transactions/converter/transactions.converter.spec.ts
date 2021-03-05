import { AccountEntity } from '@elesse/accounts';
import { CategoryEntity, enCategoryType } from '@elesse/categories';
import { EntryEntity } from '@elesse/entries';
import { TransferEntity } from '@elesse/transfers';
import { TransactionsConverter } from './transactions.converter';

describe('TransactionsParser', () => {

   it('with null accounts list should result empty list', () => {
      expect(TransactionsConverter.Convert(null, null, null, null, null, null)).toEqual([]);
   });

   it('with empty accounts list should result empty list', () => {
      expect(TransactionsConverter.Convert([], null, null, null, null, null)).toEqual([]);
   });

   it('with some accounts list should result parsed list', () => {
      const accounts: AccountEntity[] = [
         AccountEntity.Parse({ AccountID: 'ID1', Text: 'my account text' }),
         AccountEntity.Parse({ AccountID: 'ID2', Text: 'my other account text' })
      ];
      const transactionAccounts = TransactionsConverter.Convert(accounts, null, null, null, null, null);
      expect(transactionAccounts).toBeTruthy();
      expect(transactionAccounts.length).toEqual(2);
      expect(transactionAccounts[1].Account.Text).toEqual('my other account text');
   });

   it('transaction categories should have correct description extracted', () => {
      const accounts: AccountEntity[] = [
         AccountEntity.Parse({ AccountID: 'ID1', Text: 'my account text' })
      ];
      const incomeCategories: CategoryEntity[] = [
         CategoryEntity.Parse({ CategoryID: 'CAT1', HierarchyText: 'my first income category' })
      ];
      const expenseCategories: CategoryEntity[] = [
         CategoryEntity.Parse({ CategoryID: 'CAT1', HierarchyText: 'my first expense category' }),
         CategoryEntity.Parse({ CategoryID: 'CAT2', HierarchyText: 'my second expense category' })
      ];
      const entries: EntryEntity[] = [
         EntryEntity.Parse({ AccountID: 'ID1', Pattern: { Text: 'my main income', Type: enCategoryType.Income, CategoryID: 'CAT1' }, Value: 1500, DueDate: new Date(2021, 1, 2) }),
         EntryEntity.Parse({ AccountID: 'ID1', Pattern: { Text: 'my second expense', Type: enCategoryType.Expense, CategoryID: 'CAT1' }, Value: 123.45, DueDate: new Date(2021, 1, 3) }),
         EntryEntity.Parse({ AccountID: 'ID1', Pattern: { Text: 'my first expense', Type: enCategoryType.Expense, CategoryID: 'CAT2' }, Value: 300, DueDate: new Date(2021, 1, 4) }),
      ];

      const transactionAccounts = TransactionsConverter.Convert(accounts, incomeCategories, expenseCategories, entries, null, null);
      expect(transactionAccounts).toBeTruthy();
      expect(transactionAccounts.length).toEqual(1);
      expect(transactionAccounts[0].Days.length).toEqual(3);
      expect(transactionAccounts[0].Days[0].Transactions[0].Details).toEqual("my first income category");
      expect(transactionAccounts[0].Days[1].Transactions[0].Details).toEqual("my first expense category");
      expect(transactionAccounts[0].Days[2].Transactions[0].Details).toEqual("my second expense category");
   });

   it('transaction accounts should have the expected balance', () => {
      const accounts: AccountEntity[] = [
         AccountEntity.Parse({ AccountID: 'ID1', Text: 'my account text' }),
         AccountEntity.Parse({ AccountID: 'ID2', Text: 'my other account text' })
      ];
      const entries: EntryEntity[] = [
         EntryEntity.Parse({ AccountID: 'ID1', Pattern: { Text: 'my main income', Type: enCategoryType.Income }, Value: 1500, DueDate: new Date("2021-02-05"), Paid: true, PayDate: new Date("2021-02-01") }),
         EntryEntity.Parse({ AccountID: 'ID1', Pattern: { Text: 'my second expense', Type: enCategoryType.Expense }, Value: 123.45, DueDate: new Date("2021-02-15") }),
         EntryEntity.Parse({ AccountID: 'ID2', Pattern: { Text: 'my other income', Type: enCategoryType.Income }, Value: 1000, DueDate: new Date("2021-02-07") }),
         EntryEntity.Parse({ AccountID: 'ID1', Pattern: { Text: 'my first expense', Type: enCategoryType.Expense }, Value: 300, DueDate: new Date("2021-02-10"), Paid: true, PayDate: new Date("2021-02-15") }),
      ];

      const transactionAccounts = TransactionsConverter.Convert(accounts, null, null, entries, null, null);
      expect(transactionAccounts).toBeTruthy();
      expect(transactionAccounts.length).toEqual(2);
      expect(transactionAccounts[0].Balance.Realized).toEqual(1200);
      expect(transactionAccounts[0].Balance.Expected).toEqual(1076.55);
      expect(transactionAccounts[1].Balance.Realized).toEqual(0);
      expect(transactionAccounts[1].Balance.Expected).toEqual(1000);
   });

   it('transaction days should have the expected balance', () => {
      const accounts: AccountEntity[] = [
         AccountEntity.Parse({ AccountID: 'ID1', Text: 'my account text' }),
         AccountEntity.Parse({ AccountID: 'ID2', Text: 'my other account text' })
      ];
      const entries: EntryEntity[] = [
         EntryEntity.Parse({ AccountID: 'ID1', Pattern: { Text: 'my main income', Type: enCategoryType.Income }, Value: 1500, DueDate: new Date("2021-02-05"), Paid: true, PayDate: new Date("2021-02-02") }),
         EntryEntity.Parse({ AccountID: 'ID1', Pattern: { Text: 'my second expense', Type: enCategoryType.Expense }, Value: 123.45, DueDate: new Date("2021-02-15") }),
         EntryEntity.Parse({ AccountID: 'ID2', Pattern: { Text: 'my other income', Type: enCategoryType.Income }, Value: 1000, DueDate: new Date("2021-02-07") }),
         EntryEntity.Parse({ AccountID: 'ID1', Pattern: { Text: 'my first expense', Type: enCategoryType.Expense }, Value: 300, DueDate: new Date("2021-02-10"), Paid: true, PayDate: new Date("2021-02-15") }),
      ];

      const transactionAccounts = TransactionsConverter.Convert(accounts, null, null, entries, null, null);
      expect(transactionAccounts).toBeTruthy();
      expect(transactionAccounts.length).toEqual(2);
      expect(transactionAccounts[0].Days[0].Balance.Realized).toEqual(1500);
      expect(transactionAccounts[0].Days[0].Balance.Expected).toEqual(1500);
      expect(transactionAccounts[0].Days[1].Balance.Realized).toEqual(1200);
      expect(transactionAccounts[0].Days[1].Balance.Expected).toEqual(1076.55);
      expect(transactionAccounts[1].Days[0].Balance.Realized).toEqual(0);
      expect(transactionAccounts[1].Days[0].Balance.Expected).toEqual(1000);
   });

   it('transaction entries should have been sorted on expected day', () => {
      const accounts: AccountEntity[] = [
         AccountEntity.Parse({ AccountID: 'ID1', Text: 'my account text' }),
         AccountEntity.Parse({ AccountID: 'ID2', Text: 'my other account text' })
      ];
      const entries: EntryEntity[] = [
         EntryEntity.Parse({ AccountID: 'ID1', Pattern: { Text: 'my main income', Type: enCategoryType.Income }, Value: 1500, DueDate: new Date("2021-02-05"), Sorting: 0, Paid: true, PayDate: new Date("2021-02-02") }),
         EntryEntity.Parse({ AccountID: 'ID1', Pattern: { Text: 'my second expense', Type: enCategoryType.Expense }, Value: 123.45, DueDate: new Date("2021-02-15"), Sorting: 2 }),
         EntryEntity.Parse({ AccountID: 'ID2', Pattern: { Text: 'my other income', Type: enCategoryType.Income }, Value: 1000, DueDate: new Date("2021-02-07") }),
         EntryEntity.Parse({ AccountID: 'ID1', Pattern: { Text: 'my first expense', Type: enCategoryType.Expense }, Value: 300, DueDate: new Date("2021-02-10"), Sorting: 1, Paid: true, PayDate: new Date("2021-02-15") }),
      ];
      const transfers: TransferEntity[] = [
         TransferEntity.Parse({ ExpenseAccountID: 'ID1', IncomeAccountID: 'ID2', Date: new Date("2021-02-04"), Value: 54, Sorting: 3 })
      ];

      const transactionAccounts = TransactionsConverter.Convert(accounts, null, null, entries, transfers, null);

      expect(transactionAccounts).toBeTruthy();
      expect(transactionAccounts.length).toEqual(2);
      expect(transactionAccounts[0].Days.length).toEqual(3);
      expect(transactionAccounts[0].Days[0].Transactions[0].Value).toEqual(1500);
      expect(transactionAccounts[0].Days[1].Transactions[0].Value).toEqual(-54);
      expect(transactionAccounts[0].Days[2].Transactions[1].Value).toEqual(-123.45);
      expect(transactionAccounts[1].Days[0].Transactions[0].Value).toEqual(54);
      expect(transactionAccounts[1].Days[1].Transactions[0].Value).toEqual(1000);
   });

});
