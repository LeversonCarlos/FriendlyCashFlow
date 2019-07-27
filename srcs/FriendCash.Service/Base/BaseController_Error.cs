#region Using
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
#endregion

namespace FriendCash.Service.Base
{
   partial class BaseController
   {

      #region AddModelError

      protected void AddModelError(string idTranslation, string stateKey)
      { ModelState.AddModelError(stateKey, this.GetTranslation(idTranslation));  }

      protected void AddModelError(string idTranslation)
      { ModelState.AddModelError("WarningMessage", this.GetTranslation(idTranslation)); }

      #endregion

      #region GetErrorResult
      protected IHttpActionResult GetErrorResult(IdentityResult result)
      {
         if (result == null) { return InternalServerError(); }

         if (!result.Succeeded)
         {
            if (result.Errors != null) 
            { 
               foreach (string error in result.Errors) 
               { ModelState.AddModelError("", error); } 
            }

            if (ModelState.IsValid) { return BadRequest(); }
            return BadRequest(ModelState);
         }

         return null;
      }
      #endregion

      #region GetErrorResult
      protected IHttpActionResult GetErrorResult(System.Data.Entity.Validation.DbEntityValidationException value)
      {

         // INTERNAL SERVER ERROR
         if (value == null) { return InternalServerError(); }
         if (value.EntityValidationErrors == null || value.EntityValidationErrors.Count() == 0) { return InternalServerError(); }

         // ENTITY VALIDATION ERRORS
         foreach (var oResult in value.EntityValidationErrors)
         {
            if (oResult.ValidationErrors != null && oResult.ValidationErrors.Count != 0)
            {
               foreach (var oError in oResult.ValidationErrors)
               { ModelState.AddModelError("", oError.ErrorMessage); }
            }
         }

         // RESULT
         if (ModelState.IsValid) { return BadRequest(); }
         return BadRequest(ModelState);

      }
      #endregion

      #region GetErrorResult
      protected IHttpActionResult GetErrorResult(Exception value)
      {

         // INTERNAL SERVER ERROR
         if (value == null) { return InternalServerError(); }

         // RESULT
         ModelState.AddModelError("", value); 
         return BadRequest(ModelState);

      }
      #endregion

   }
}