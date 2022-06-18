using Lewio.CashFlow.Services;
namespace Lewio.CashFlow.Accounts;
#nullable disable

public class SaveResponseModel : SharedResponseModel
{
   public AccountEntity Data { get; set; }
}
