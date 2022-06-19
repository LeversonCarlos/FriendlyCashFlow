using System.ComponentModel.DataAnnotations;
using Lewio.CashFlow.Shared;
namespace Lewio.CashFlow.Accounts;
#nullable disable

public class LoadRequestModel : SharedRequestModel
{
   [Required]
   public Guid AccountID { get; set; }
}
