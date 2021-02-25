using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Elesse.Balances
{

   internal partial class BalanceEntity : IBalanceEntity
   {

      Shared.EntityID _BalanceID;
      [BsonId]
      public Shared.EntityID BalanceID
      {
         get => _BalanceID;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNINGS.INVALID_BALANCEID);
            _BalanceID = value;
         }
      }

      Shared.EntityID _AccountID;
      public Shared.EntityID AccountID
      {
         get => _AccountID;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNINGS.INVALID_ACCOUNTID);
            _AccountID = value;
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

      decimal _ExpectedValue;
      public decimal ExpectedValue
      {
         get => _ExpectedValue;
         internal set
         {
            _ExpectedValue = value;
         }
      }

      decimal _RealizedValue;
      public decimal RealizedValue
      {
         get => _RealizedValue;
         internal set
         {
            _RealizedValue = value;
         }
      }

   }

   partial struct WARNINGS
   {
      internal const string INVALID_BALANCEID = "INVALID_BALANCEID_PROPERTY";
      internal const string INVALID_ACCOUNTID = "INVALID_ACCOUNTID_PROPERTY";
      internal const string INVALID_DATE = "INVALID_DATE_PROPERTY";
   }

}
