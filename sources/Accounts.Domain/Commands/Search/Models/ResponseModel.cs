using Lewio.CashFlow.Services;
namespace Lewio.CashFlow.Accounts;
#nullable disable

public class SearchResponseModel : SharedResponseModel
{
   public AccountEntity[] DataList { get; set; }
}
