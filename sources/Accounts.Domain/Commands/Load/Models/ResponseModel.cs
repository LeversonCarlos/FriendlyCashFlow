using Lewio.CashFlow.Shared;
namespace Lewio.CashFlow.Accounts;
#nullable disable

public class LoadResponseModel : SharedResponseModel
{
   public AccountEntity Data { get; set; }
}
