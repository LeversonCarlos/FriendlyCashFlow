namespace Import
{

   public class AppSettings
   {
      public AppSettings_Api Api { get; set; }
   }

   public class AppSettings_Api
   {
      public string Url { get; set; }
      public string Username { get; set; }
      public string Password { get; set; }
   }

}
