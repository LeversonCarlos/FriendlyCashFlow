#region Using
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

[assembly: Microsoft.Owin.OwinStartup(typeof(FriendCash.Service.Startup))]
namespace FriendCash.Service
{
   public partial class Startup
   {

      public void Configuration(IAppBuilder app)
      {
         this.ConfigureAuth(app);
      }

   }
}