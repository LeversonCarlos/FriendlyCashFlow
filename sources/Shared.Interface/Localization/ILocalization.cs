namespace Lewio.CashFlow.Shared
{
   public interface ILocalization : IDisposable
   {
      Task<string> GetString(string value);
   }
}
