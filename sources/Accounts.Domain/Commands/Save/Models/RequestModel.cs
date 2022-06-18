using System.ComponentModel.DataAnnotations;
using Lewio.CashFlow.Services;
namespace Lewio.CashFlow.Accounts;
#nullable disable

public class SaveRequestModel : SharedRequestModel
{
   [Required]
   public AccountEntity Data { get; set; }
}
