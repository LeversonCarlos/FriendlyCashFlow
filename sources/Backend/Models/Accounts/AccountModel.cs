namespace Lewio.CashFlow.Accounts;

public class AccountModel
{

   public EntityID AccountID { get; set; }

   public string Text { get; set; }

   public AccountTypeEnum Type { get; set; }

   public short? ClosingDay { get; set; }
   public short? DueDay { get; set; }
   public DateOnly? DueDate { get; set; }

   public bool Active { get; set; }

}
