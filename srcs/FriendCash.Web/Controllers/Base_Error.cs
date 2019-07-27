#region Using
using FriendCash.Service.Base;
using System.Web.Mvc;
#endregion

namespace FriendCash.Web.Controllers
{
   partial class Base
   {

      internal ViewResult ErrorView<T>(Bundle<T> oBundle)
      {
         ViewBag.BundleMessages = oBundle.Messages;
         return View("Error");
      }

   }
}