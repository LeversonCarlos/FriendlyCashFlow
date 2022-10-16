namespace Lewio.CashFlow.UnitTests;

public partial class BaseTests : IDisposable
{

   protected Bogus.Faker _Faker = new Bogus.Faker();

   public virtual void Dispose()
   {
   }

}
