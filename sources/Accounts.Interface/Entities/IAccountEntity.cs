namespace Lewio.CashFlow.Accounts;

public enum AccountEntityTypeEnum : short
{
   General = 0,
   Bank = 1,
   CreditCard = 2,
   Investment = 3,
   Service = 4
}

public interface IAccountEntity
{
   Guid? AccountID { get; set; }

   AccountEntityTypeEnum Type { get; set; }
   string Text { get; set; }

   short? CreditCardClosingDay { get; set; }
   short? CreditCardDueDay { get; set; }

   bool IsActive { get; set; }
}
