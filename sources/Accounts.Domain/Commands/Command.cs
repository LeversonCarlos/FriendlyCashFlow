using Lewio.CashFlow.Repository;
using Lewio.CashFlow.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.Accounts;

public abstract class AccountsCommand<TRequest, TResponse> : SharedCommand<TRequest, TResponse>
   where TRequest : SharedRequestModel
   where TResponse : SharedResponseModel
{

   public AccountsCommand(IServiceProvider serviceProvider) : base(serviceProvider) =>
      _AccountRepository = serviceProvider.GetService<IAccountRepository>()!;
   protected readonly IAccountRepository _AccountRepository;

}
