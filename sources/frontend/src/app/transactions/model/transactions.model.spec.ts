import { EntryEntity } from '@elesse/entries';
import { Balance, TransactionAccount, TransactionDay, TransactionEntry } from './transactions.model';

describe('TransactionAccount', () => {
   it('should create an instance', () => {
      expect(new TransactionAccount()).toBeTruthy();
   });
});

describe('TransactionDay', () => {
   it('should create an instance', () => {
      expect(new TransactionDay()).toBeTruthy();
   });
});

describe('Balance', () => {
   it('should create an instance', () => {
      expect(new Balance()).toBeTruthy();
   });
});

describe('TransactionEntry', () => {

   it('should create an instance', () => {
      expect(new TransactionEntry()).toBeTruthy();
   });

   it('should parse paid entry entity', () => {
      const text = 'my pattern text';
      const dueDate = new Date('2021-02-14');
      const value = 12.34;
      const paid = true;
      const payDate = new Date('2021-02-10');
      const entry = EntryEntity.Parse({ Pattern: { Text: text }, DueDate: dueDate, EntryValue: value, Paid: paid, PayDate: payDate });

      const transaction = TransactionEntry.Parse(entry);

      expect(transaction.Text).toEqual(text);
      expect(transaction.Date).toEqual(payDate);
      expect(transaction.Value).toEqual(value);
      expect(transaction.Entry.DueDate).toEqual(dueDate);
   });

   it('should parse unpaid entry entity', () => {
      const text = 'my other pattern text';
      const dueDate = new Date('2021-02-14');
      const value = 43.21;
      const paid = false;
      const entry = EntryEntity.Parse({ Pattern: { Text: text }, DueDate: dueDate, EntryValue: value, Paid: paid });

      const transaction = TransactionEntry.Parse(entry);

      expect(transaction.Text).toEqual(text);
      expect(transaction.Date).toEqual(dueDate);
      expect(transaction.Value).toEqual(value);
      expect(transaction.Entry.Pattern.Text).toEqual(text);
   });

   it('should parse without errors even with a null pattern', () => {
      const dueDate = new Date('2021-02-14');
      const value = 43.21;
      const entry = EntryEntity.Parse({ DueDate: dueDate, EntryValue: value });

      const transaction = TransactionEntry.Parse(entry);

      expect(transaction.Text).toEqual('');
      expect(transaction.Date).toEqual(dueDate);
      expect(transaction.Value).toEqual(value);
   });

});
