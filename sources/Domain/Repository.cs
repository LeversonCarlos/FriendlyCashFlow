namespace Lewio.CashFlow.Repository;

public interface IRepository : IDisposable
{
   IMainRepository Main { get; }
}

public partial interface IMainRepository : IRepository
{

}
