namespace Lewio.CashFlow.Domain.Accounts;

public partial class AccountEntity : IAccount
{
   public Guid? AccountID { get; set; }

   public AccountTypeEnum Type { get; set; }
   public string Text { get; set; } = "";

   public short? CreditCardClosingDay { get; set; }
   public short? CreditCardDueDay { get; set; }

   public bool IsActive { get; set; } = false;
}
