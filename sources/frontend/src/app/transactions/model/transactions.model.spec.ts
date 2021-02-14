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
});
