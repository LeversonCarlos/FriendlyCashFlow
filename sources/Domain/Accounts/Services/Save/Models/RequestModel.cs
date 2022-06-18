using System.ComponentModel.DataAnnotations;
using Lewio.CashFlow.Services;
#nullable disable

namespace Lewio.CashFlow.Domain.Accounts.Services;

public class SaveRequestModel : SharedRequestModel
{
   [Required]
   public AccountEntity Data { get; set; }
}
