namespace Elesse.Identity
{
   public interface IUser
   {
      string UserID { get; }
      string UserName { get; }
      string[] Roles { get; }
   }
}