using Lewio.CashFlow.Domain.Accounts;

namespace Lewio.CashFlow.Repository;

partial interface IAccountRepository
{
   Task<IAccount> SaveNew(IAccount value);
}
