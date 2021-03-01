using System;
using System.Threading.Tasks;

namespace Elesse.Balances
{
   partial class BalanceService
   {

      public async Task<IBalanceEntity> IncreaseAsync(Shared.EntityID accountID, DateTime date, decimal value, bool paid)
      {

         // VALIDATE PARAMETERS
         if (accountID == null)
            throw new ArgumentException(WARNINGS.INVALID_ACCOUNTID);
         if (date == null || date == DateTime.MinValue)
            throw new ArgumentException(WARNINGS.INVALID_DATE);

         // LOAD BALANCE
         var balance = ((BalanceEntity)await _BalanceRepository.LoadAsync(accountID, date));
         var expectedValue = value;
         var realizedValue = paid ? value : 0;

         // INCREASE BALANCE VALUES
         if (balance != null)
         {
            balance.ExpectedValue += expectedValue;
            balance.RealizedValue += realizedValue;
            await _BalanceRepository.UpdateAsync(balance);
         }

         // ADD NEW BALANCE
         if (balance == null)
         {
            balance = BalanceEntity.Create(accountID, date, expectedValue, realizedValue);
            await _BalanceRepository.InsertAsync(balance);
         }

         // RESULT
         return balance;
      }

   }
}
