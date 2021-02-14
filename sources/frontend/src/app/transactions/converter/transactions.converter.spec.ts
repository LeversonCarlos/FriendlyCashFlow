import { AccountEntity } from '@elesse/accounts';
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
         AccountEntity.Parse({ Text: 'my account text' }),
         AccountEntity.Parse({ Text: 'my other account text' })
      ];
      const transactionAccounts = TransactionsConverter.Convert(accounts, null);
      expect(transactionAccounts).toBeTruthy();
      expect(transactionAccounts.length).toEqual(2);
      expect(transactionAccounts[1].Account.Text).toEqual('my other account text');
   });

});
