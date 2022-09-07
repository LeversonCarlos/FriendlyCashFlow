namespace Lewio.CashFlow.Accounts;

partial class ModelExtensions
{

   private static DateOnly? GetDueDate(this AccountEntity entity)
   {
      if (entity == null)
         return null;

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
