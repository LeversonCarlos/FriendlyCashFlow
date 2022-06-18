using Lewio.CashFlow.Accounts;
namespace Lewio.CashFlow.Repository;

public partial interface IAccountRepository : IDisposable
{
   Task<IAccountEntity> GetNew();
   Task<IAccountEntity> GetByID(Guid id);
   Task<bool> Save(IAccountEntity value);
}
