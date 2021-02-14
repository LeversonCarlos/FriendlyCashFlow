import { AccountEntity } from '@elesse/accounts';
import { enCategoryType } from '@elesse/categories';
import { EntryEntity } from '@elesse/entries';
import { TransactionsConverter } from './transactions.converter';

describe('TransactionsParser', () => {

   it('with null accounts list should result empty list', () => {
      expect(TransactionsConverter.Convert(null, null)).toEqual([]);
   });

   it('with empty accounts list should result empty list', () => {
      expect(TransactionsConverter.Convert([], null)).toEqual([]);
   });

   it('with some accounts list should result parsed list', () => {
      const accounts: AccountEntity[] = [
         AccountEntity.Parse({ AccountID: 'ID1', Text: 'my account text' }),
         AccountEntity.Parse({ AccountID: 'ID2', Text: 'my other account text' })
      ];
      const transactionAccounts = TransactionsConverter.Convert(accounts, null);
      expect(transactionAccounts).toBeTruthy();
      expect(transactionAccounts.length).toEqual(2);
      expect(transactionAccounts[1].Account.Text).toEqual('my other account text');
   });

   it('with some accounts and entries should result parsed list', () => {
      const accounts: AccountEntity[] = [
         AccountEntity.Parse({ AccountID: 'ID1', Text: 'my account text' }),
         AccountEntity.Parse({ AccountID: 'ID2', Text: 'my other account text' })
      ];
      const entries: EntryEntity[] = [
         EntryEntity.Parse({ AccountID: 'ID1', Pattern: { Text: 'my main income', Type: enCategoryType.Income }, EntryValue: 1500, DueDate: new Date("2021-02-05"), Paid: true, PayDate: new Date("2021-02-01") }),
         EntryEntity.Parse({ AccountID: 'ID1', Pattern: { Text: 'my second expense', Type: enCategoryType.Expense }, EntryValue: 123.45, DueDate: new Date("2021-02-15") }),
         EntryEntity.Parse({ AccountID: 'ID2', Pattern: { Text: 'my other income', Type: enCategoryType.Income }, EntryValue: 1000, DueDate: new Date("2021-02-07") }),
         EntryEntity.Parse({ AccountID: 'ID1', Pattern: { Text: 'my first expense', Type: enCategoryType.Expense }, EntryValue: 300, DueDate: new Date("2021-02-10"), Paid: true, PayDate: new Date("2021-02-08") }),
      ];

      const transactionAccounts = TransactionsConverter.Convert(accounts, entries);
      expect(transactionAccounts).toBeTruthy();
      expect(transactionAccounts.length).toEqual(2);
      expect(transactionAccounts[0].Balance.Realized).toEqual(1200);
      expect(transactionAccounts[0].Balance.Expected).toEqual(1076.55);
      expect(transactionAccounts[1].Balance.Realized).toEqual(0);
      expect(transactionAccounts[1].Balance.Expected).toEqual(1000);
   });

});
