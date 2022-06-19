using Lewio.CashFlow.Shared;
namespace Lewio.CashFlow.Accounts;
#nullable disable

public class SaveResponseModel : SharedResponseModel
{
   public AccountEntity Data { get; set; }
}
