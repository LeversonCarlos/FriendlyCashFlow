using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Elesse.Transfers
{

   internal partial class TransferEntity : ITransferEntity
   {

      Shared.EntityID _TransferID;
      [BsonId]
      public Shared.EntityID TransferID
      {
         get => _TransferID;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNINGS.INVALID_TRANSFERID);
            _TransferID = value;
         }
      }

      Shared.EntityID _ExpenseAccountID;
      public Shared.EntityID ExpenseAccountID
      {
         get => _ExpenseAccountID;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNINGS.INVALID_EXPENSEACCOUNTID);
            _ExpenseAccountID = value;
         }
      }

      Shared.EntityID _IncomeAccountID;
      public Shared.EntityID IncomeAccountID
      {
         get => _IncomeAccountID;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNINGS.INVALID_INCOMEACCOUNTID);
            if (value == _ExpenseAccountID)
               throw new ArgumentException(WARNINGS.INVALID_EQUAL_EXPENSE_AND_INCOME_ACCOUNTS);
            _IncomeAccountID = value;
         }
      }

      DateTime _Date;
      public DateTime Date
      {
         get => _Date;
         private set
         {
            if (value == null || value == DateTime.MinValue)
               throw new ArgumentException(WARNINGS.INVALID_DATE);
            _Date = value;
         }
      }

      decimal _Value;
      public decimal Value
      {
         get => _Value;
         private set
         {
            if (value <= 0)
               throw new ArgumentException(WARNINGS.INVALID_VALUE);
            _Value = value;
         }
      }

   }

   partial struct WARNINGS
   {
      internal const string INVALID_TRANSFERID = "INVALID_TRANSFERID";
      internal const string INVALID_EXPENSEACCOUNTID = "INVALID_EXPENSEACCOUNTID";
      internal const string INVALID_INCOMEACCOUNTID = "INVALID_INCOMEACCOUNTID";
      internal const string INVALID_EQUAL_EXPENSE_AND_INCOME_ACCOUNTS = "INVALID_EQUAL_EXPENSE_AND_INCOME_ACCOUNTS";
      internal const string INVALID_DATE = "INVALID_DATE";
      internal const string INVALID_VALUE = "INVALID_VALUE";
   }

}
