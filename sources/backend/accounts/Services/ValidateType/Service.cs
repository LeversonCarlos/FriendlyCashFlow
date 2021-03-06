using System.Threading.Tasks;

namespace Elesse.Accounts
{
   partial class AccountService
   {

      internal async Task<string[]> ValidateTypeAsync(enAccountType type, short? closingDay, short? dueDay)
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

   }
}
