namespace Lewio.CashFlow.Accounts;

internal static class AccountEntityExtensions
{

   public static AccountModel ToModel(this AccountEntity entity) =>
      new AccountModel
      {
         AccountID = entity.AccountID,
         Text = entity.Text,
         Type = (AccountTypeEnum)entity.Type,
         ClosingDay = entity.ClosingDay,
         DueDay = entity.DueDay,
         DueDate = ToModel_GetDueDate(entity),
         Active = entity.Active
      };

   private static DateOnly? ToModel_GetDueDate(this AccountEntity entity)
   {
      if (entity.Type != (short)AccountTypeEnum.CreditCard)
         return null;
      if (!entity.DueDay.HasValue || entity.DueDay.Value == 0)
         return null;
      if (!entity.ClosingDay.HasValue || entity.ClosingDay.Value == 0)
         return null;

      var dueDay = entity.DueDay.Value;
      var closingDay = entity.ClosingDay.Value;
      var now = DateTime.Now;
      var dueDate = new DateOnly(now.Year, now.Month, dueDay);

      if (now.Day < closingDay && dueDate.Day < closingDay)
         dueDate = dueDate.AddMonths(1);
      else if (now.Day >= closingDay && dueDate.Day < closingDay)
         dueDate = dueDate.AddMonths(1);
      else if (now.Day >= closingDay)
         dueDate = dueDate.AddMonths(1);

      return dueDate;

   }

}
