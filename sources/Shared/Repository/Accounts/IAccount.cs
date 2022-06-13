namespace Lewio.CashFlow.Domain.Accounts;

public enum AccountTypeEnum : short
{
   General = 0,
   Bank = 1,
   CreditCard = 2,
   Investment = 3,
   Service = 4
}

public interface IAccount
{

   Guid ID { get; set; }
   AccountTypeEnum Type { get; set; }

   string Text { get; set; }

}
