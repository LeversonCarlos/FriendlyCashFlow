namespace Lewio.CashFlow.Repository;

public partial interface IAccountRepository : IRepository
{
}

partial interface IMainRepository
{
   IAccountRepository Accounts { get; }
}
