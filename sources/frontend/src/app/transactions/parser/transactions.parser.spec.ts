import { TransactionsParser } from './transactions.parser';

describe('TransactionsParser', () => {

   it('with null accounts list should result empty list', () => {
      expect(TransactionsParser.Parse(null, null)).toEqual([]);
   });

   it('with empty accounts list should result empty list', () => {
      expect(TransactionsParser.Parse([], null)).toEqual([]);
   });

});
