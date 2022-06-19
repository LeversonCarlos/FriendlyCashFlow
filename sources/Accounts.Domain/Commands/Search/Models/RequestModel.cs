using Lewio.CashFlow.Shared;
namespace Lewio.CashFlow.Accounts;
#nullable disable

public class SearchRequestModel : SharedRequestModel
{
   public string SearchTerms { get; set; }
}
