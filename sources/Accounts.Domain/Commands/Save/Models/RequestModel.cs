using System.ComponentModel.DataAnnotations;
using Lewio.CashFlow.Shared;
namespace Lewio.CashFlow.Accounts;
#nullable disable

public class SaveRequestModel : SharedRequestModel
{
   [Required]
   public AccountEntity Data { get; set; }
}
