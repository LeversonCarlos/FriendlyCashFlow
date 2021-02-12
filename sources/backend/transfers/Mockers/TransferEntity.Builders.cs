using System;

namespace Elesse.Transfers
{
   partial class TransferEntity
   {
      public static Tests.TransferEntityBuilder Builder() => new Tests.TransferEntityBuilder();
   }
}
namespace Elesse.Transfers.Tests
{
   internal class TransferEntityBuilder
   {

      Shared.EntityID _TransferID = Shared.EntityID.MockerID();
      public TransferEntityBuilder WithTransferID(Shared.EntityID transferID)
      {
         _TransferID = transferID;
         return this;
      }

      Shared.EntityID _ExpenseAccountID = Shared.EntityID.MockerID();
      public TransferEntityBuilder WithExpenseAccountID(Shared.EntityID accountID)
      {
         _ExpenseAccountID = accountID;
         return this;
      }

      Shared.EntityID _IncomeAccountID = Shared.EntityID.MockerID();
      public TransferEntityBuilder WithIncomeAccountID(Shared.EntityID accountID)
      {
         _IncomeAccountID = accountID;
         return this;
      }

      DateTime _Date = Shared.Faker.GetFaker().Date.Soon();
      public TransferEntityBuilder WithDate(DateTime date)
      {
         _Date = date;
         return this;
      }

      decimal _Value = Shared.Faker.GetFaker().Random.Decimal(0, 10000);
      public TransferEntityBuilder WithValue(decimal value)
      {
         _Value = value;
         return this;
      }

      public TransferEntity Build() =>
         TransferEntity.Restore(_TransferID, _ExpenseAccountID, _IncomeAccountID, _Date, _Value);

   }
}
