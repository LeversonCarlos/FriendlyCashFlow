using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
   public class IncomesController : DocumentsController
   {

      #region New
      public IncomesController() : base(Model.Document.enType.Income)
      { }
      #endregion

   }
}
