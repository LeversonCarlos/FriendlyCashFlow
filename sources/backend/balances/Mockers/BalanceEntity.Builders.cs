using System;

namespace Elesse.Balances
{
   partial class BalanceEntity
   {
      internal static Tests.BalanceEntityBuilder Builder() => new Tests.BalanceEntityBuilder();
   }
}
namespace Elesse.Balances.Tests
{
   internal class BalanceEntityBuilder
   {

      Shared.EntityID _BalanceID = Shared.EntityID.MockerID();
      public BalanceEntityBuilder WithBalanceID(Shared.EntityID balanceID)
      {
         _BalanceID = balanceID;
         return this;
      }

      Shared.EntityID _AccountID = Shared.EntityID.MockerID();
      public BalanceEntityBuilder WithAccountID(Shared.EntityID accountID)
      {
         _AccountID = accountID;
         return this;
      }

      DateTime _Date = Shared.Faker.GetFaker().Date.Soon();
      public BalanceEntityBuilder WithDate(DateTime date)
      {
         _Date = date;
         return this;
      }

      decimal _ExpectedValue = Shared.Faker.GetFaker().Random.Decimal(0, 10000);
      public BalanceEntityBuilder WithExpectedValue(decimal expectedValue)
      {
         _ExpectedValue = expectedValue;
         return this;
      }

      decimal _RealizedValue = Shared.Faker.GetFaker().Random.Decimal(0, 10000);
      public BalanceEntityBuilder WithRealizedValue(decimal realizedValue)
      {
         _RealizedValue = realizedValue;
         return this;
      }

      public BalanceEntity Build() =>
         BalanceEntity.Restore(_BalanceID, _AccountID, _Date, _ExpectedValue, _RealizedValue);

   }
}
