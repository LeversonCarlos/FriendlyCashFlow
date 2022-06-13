using Lewio.CashFlow.Domain.Accounts;

namespace Lewio.CashFlow.Repository;

partial interface IAccountRepository
{
   Task<AccountEntity> SaveNew(AccountEntity value);
}
