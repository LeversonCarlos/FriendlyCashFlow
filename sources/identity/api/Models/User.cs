namespace Elesse.Identity
{
   internal class User : IUser
   {

      public string UserID { get; set; }
      public string UserName { get; set; }

      public string[] Roles { get; set; }

   }
}
