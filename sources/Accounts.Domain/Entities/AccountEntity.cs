namespace Lewio.CashFlow.Accounts;

public partial class AccountEntity : IAccountEntity
{
   public Guid? AccountID { get; set; }

   public AccountEntityTypeEnum Type { get; set; }
   public string Text { get; set; } = "";

   public short? CreditCardClosingDay { get; set; }
   public short? CreditCardDueDay { get; set; }

   public bool IsActive { get; set; } = false;
}
