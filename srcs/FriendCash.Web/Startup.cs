#region Using
using Microsoft.Owin;
using Owin;
#endregion

[assembly: OwinStartupAttribute(typeof(FriendCash.Web.Startup))]
namespace FriendCash.Web
{
   public partial class Startup
   {

      public void Configuration(IAppBuilder app)
      {
         this.ConfigureAuth(app);
      }

   }
}
