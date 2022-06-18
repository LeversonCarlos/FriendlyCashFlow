using Lewio.CashFlow.Services;
#nullable disable

namespace Lewio.CashFlow.Domain.Accounts.Services;

public class SaveResponseModel : SharedResponseModel
{
   public AccountEntity Data { get; set; }
}
