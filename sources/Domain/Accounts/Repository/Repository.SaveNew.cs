using Lewio.CashFlow.Domains.Accounts;

namespace Lewio.CashFlow.Repository;

partial interface IAccountRepository
{
   Task<AccountEntity> SaveNew(AccountEntity value);
}
