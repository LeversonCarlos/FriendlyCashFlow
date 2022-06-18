using Lewio.CashFlow.Domain.Accounts;

namespace Lewio.CashFlow.Repository;

public partial interface IAccountRepository : IRepository
{
   Task<IAccount> GetNew();
   Task<IAccount> GetByID(Guid id);
   Task<bool> Save(IAccount value);
}

partial interface IMainRepository
{
   IAccountRepository Accounts { get; }
}
