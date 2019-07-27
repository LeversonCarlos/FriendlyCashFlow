using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FriendCash.Web.Controllers
{
   public class ExpensesController : DocumentsController
   {

      #region Initialize
      public ExpensesController()
         : base(Model.Document.enType.Expense)
      { }
      #endregion

   }
}
