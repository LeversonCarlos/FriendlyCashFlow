#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
#endregion

namespace FriendCash.Service.Configs
{
   public class ValidateModelAttribute : ActionFilterAttribute
   {
      public override void OnActionExecuting(HttpActionContext actionContext)
      {
         try
         {
            if (actionContext.ModelState.IsValid == false)
            {
               var oModelState = new ModelStateDictionary();
               var idLanguage = actionContext.Request.Headers.AcceptLanguage.Select(x => x.Value).FirstOrDefault();
               foreach (var sStateKey in actionContext.ModelState.Keys)
               {
                  var oOriginalState = actionContext.ModelState[sStateKey];
                  if (oOriginalState.Errors.Any())
                  {
                     var oState = new ModelState();
                     foreach (var oOriginalError in oOriginalState.Errors)
                     { oState.Errors.Add(Base.BaseController.GetTranslation(idLanguage, oOriginalError.ErrorMessage)); }
                     oModelState.Add(sStateKey, oState);
                  }
               }
               actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, oModelState);
            }
         }
         catch { }
      }
   }
}