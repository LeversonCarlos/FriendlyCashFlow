using System;
using System.Threading.Tasks;

namespace Elesse.Accounts
{

   partial class AccountService
   {

      internal async Task<string[]> ValidateTypeAsync(enAccountType type, short? closingDay, short? dueDay)
      {
         try
         {
            if (type != enAccountType.CreditCard)
            {
               if (closingDay.HasValue || dueDay.HasValue)
                  return new string[] { WARNINGS.DAYS_ONLY_VALID_FOR_CREDIT_CARD_TYPE };
            }
            else
            {
               if (!closingDay.HasValue || !dueDay.HasValue)
                  return new string[] { WARNINGS.DAYS_REQUIRED_FOR_CREDIT_CARD_TYPE };
            }

            await Task.CompletedTask;
            return new string[] { };
         }
         catch (Exception) { return new string[] { WARNINGS.INVALID_TYPE }; }
      }

   }

   partial struct WARNINGS
   {
      internal const string INVALID_TYPE = "WARNING_ACCOUNTS_INVALID_TYPE";
      internal const string DAYS_ONLY_VALID_FOR_CREDIT_CARD_TYPE = "WARNING_ACCOUNTS_DAYS_ONLY_VALID_FOR_CREDIT_CARD_TYPE";
      internal const string DAYS_REQUIRED_FOR_CREDIT_CARD_TYPE = "WARNING_ACCOUNTS_DAYS_REQUIRED_FOR_CREDIT_CARD_TYPE";
   }

}
