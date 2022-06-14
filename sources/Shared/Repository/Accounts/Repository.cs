using Lewio.CashFlow.Domain.Accounts;

namespace Lewio.CashFlow.Repository;

public partial interface IAccountRepository : IRepository
{
   Task<IAccount> GetByID(Guid id);
   Task<bool> SaveNew(IAccount value);
}

partial interface IMainRepository
{
   IAccountRepository Accounts { get; }
}
