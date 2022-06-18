using Lewio.CashFlow.Repository;
using Lewio.CashFlow.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.Accounts;

public abstract class AccountsService<TRequest, TResponse> : SharedService<TRequest, TResponse>
   where TRequest : SharedRequestModel
   where TResponse : SharedResponseModel
{

   public AccountsService(IServiceProvider serviceProvider) : base(serviceProvider) =>
      _AccountRepository = serviceProvider.GetService<IAccountRepository>()!;
   protected readonly IAccountRepository _AccountRepository;

}
