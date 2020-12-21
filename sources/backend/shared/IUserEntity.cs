namespace Elesse.Identity
{
   public interface IUserEntity
   {
      string UserID { get; }
      string UserName { get; }
      string Password { get; }
   }
}