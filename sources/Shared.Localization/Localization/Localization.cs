namespace Lewio.CashFlow.Shared
{
   public class Localization : ILocalization
   {

      public Task<string> GetString(string value) =>
         Task.FromResult(value);

      public void Dispose()
      {
      }

   }
}
