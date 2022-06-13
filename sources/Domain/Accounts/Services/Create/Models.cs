using System.ComponentModel.DataAnnotations;
using Lewio.CashFlow.Services;

namespace Lewio.CashFlow.Domain.Accounts.Services;

public class CreateRequestModel : SharedRequestModel
{
   [Required]
   public AccountTypeEnum Type { get; set; }
}

public class CreateResponseModel : SharedResponseModel
{
   public AccountEntity? Data { get; set; }
}
