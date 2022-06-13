using System.ComponentModel.DataAnnotations;
using Lewio.CashFlow.Services;

namespace Lewio.CashFlow.Domains.Accounts.Services;

public class CreateRequestModel : SharedRequestModel
{
   [Required]
   public AccountTypeEnum Type { get; set; }
}

public class CreateResponseModel : SharedResponseModel
{
   public AccountEntity? Data { get; set; }
}
