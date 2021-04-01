namespace Elesse.Shared
{
   public interface IBusinessRule
   {
      bool IsBroken();
      string Message { get; }
   }
}
