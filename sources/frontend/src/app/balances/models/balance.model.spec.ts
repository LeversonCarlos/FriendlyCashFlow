import { BalanceEntityMocker } from './balance.mocker.spec';
import { BalanceEntity } from './balance.model';

describe('BalanceEntity', () => {

   it('should create an instance', () => {
      expect(new BalanceEntity()).toBeTruthy();
   });

   it('should parse anonymous type and materialize properties', () => {
      const balanceID = "my balance id";
      const accountID = "my account id";
      const date = new Date("2021-01-23");
      const expectedValue = 12.34;
      const realizedValue = 43.21;

      const entry = BalanceEntityMocker
         .Create()
         .WithBalanceID(balanceID)
         .WithAccountID(accountID)
         .WithDate(date)
         .WithExpectedValue(expectedValue)
         .WithRealizedValue(realizedValue)
         .Build();

      expect(entry.BalanceID).toEqual(balanceID);
      expect(entry.AccountID).toEqual(accountID);
      expect(entry.Date).toEqual(date);
      expect(entry.ExpectedValue).toEqual(expectedValue);
      expect(entry.RealizedValue).toEqual(realizedValue);
   });

});
