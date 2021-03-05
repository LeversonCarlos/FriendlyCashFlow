import { BalanceEntity } from "./balance.model";

export class BalanceEntityMocker {
   public static Create(): BalanceEntityMocker { return new BalanceEntityMocker(); }

   private _BalanceEntity: BalanceEntity = BalanceEntity.Parse({
      BalanceID: 'BalanceID',
      AccountID: 'AccountID',
      Date: new Date(),
      ExpectedValue: 1234.56,
      RealizedValue: 6543.21
   });

   public WithBalanceID(BalanceID: string): BalanceEntityMocker {
      this._BalanceEntity.BalanceID = BalanceID;
      return this;
   }

   public WithAccountID(accountID: string): BalanceEntityMocker {
      this._BalanceEntity.AccountID = accountID;
      return this;
   }

   public WithDate(date: Date): BalanceEntityMocker {
      this._BalanceEntity.Date = date;
      return this;
   }

   public WithExpectedValue(expectedValue: number): BalanceEntityMocker {
      this._BalanceEntity.ExpectedValue = expectedValue;
      return this;
   }

   public WithRealizedValue(realizedValue: number): BalanceEntityMocker {
      this._BalanceEntity.RealizedValue = realizedValue;
      return this;
   }

   public Build(): BalanceEntity { return this._BalanceEntity; }
}

