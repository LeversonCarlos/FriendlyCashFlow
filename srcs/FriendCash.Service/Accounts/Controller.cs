#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;
#endregion

namespace FriendCash.Service.Accounts
{

   [RoutePrefix("api/accounts")]
   public class AccountController : Base.BaseController
   {

      #region Contants
      internal class Constants
      {
         internal const string WARNING_TEXT_DUPLICITY = "MSG_ACCOUNTS_TEXT_DUPLICITY";
         internal const string WARNING_ACCOUNT_NOTFOUND = "MSG_ACCOUNTS_ACCOUNT_NOTFOUND";
         internal const string WARNING_CLOSINGDAY_REQUIRED = "MSG_ACCOUNTS_CLOSINGDAY_REQUIRED";
         internal const string WARNING_DUEDAY_REQUIRED = "MSG_ACCOUNTS_DUEDAY_REQUIRED";
      }
      #endregion

      #region Query

      internal IQueryable<Model.bindAccount> QueryData()
      {
         var idUser = this.GetUserID();
         return this.DataContext.Accounts
            .Where(x => x.RowStatusValue == (short)Base.BaseModel.enRowStatus.Active && x.idUser == idUser)
            .AsQueryable();
      }

      internal IQueryable<Model.viewAccount> QueryView()
      {
         return this.QueryData()
            .Select(x => new Model.viewAccount()
            {
               idAccount = x.idAccount,
               Text = x.Text,
               TypeValue = x.TypeValue,
               Active = x.Active,
               ClosingDay = x.ClosingDay,
               DueDay = x.DueDay
            })
            .OrderBy(x => x.Text)
            .AsQueryable();
      }

      #endregion


      #region GetAll
      [Authorize(Roles = "User,Viewer")]
      [Route("")]
      public async Task<IHttpActionResult> GetAll()
      {
         try
         {
            var oQuery = this.QueryView();
            var oData = await Task.FromResult(oQuery.ToList());
            return Ok(oData);

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region GetSingle
      [Authorize(Roles = "User,Viewer")]
      [Route("{id:long}", Name = "GetAccountByID")]
      public async Task<IHttpActionResult> GetSingle(long id)
      {
         try
         {
            var oQuery = this.QueryView().Where(x => x.idAccount == id);
            var oData = await Task.FromResult(oQuery.FirstOrDefault());
            return Ok(oData);

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region GetRelated

      [Authorize(Roles = "User,Viewer")]
      [Route("related/{searchID:long}")]
      [Route("related/{searchText}/")]
      [Route("related")]
      public async Task<IHttpActionResult> GetRelatedInner(long searchID = 0, string searchText = "")
      {
         try
         {

            // MAIN QUERY
            var dataQuery = this.QueryData().Where(x => x.Active == true);

            // FILTER: ID
            if (searchID > 0)
            { dataQuery = dataQuery.Where(x => x.idAccount == searchID); }

            // FILTER: TEXT
            if (!string.IsNullOrEmpty(searchText))
            { dataQuery = dataQuery.Where(x => x.Text.Contains(searchText)); }

            // ORDER
            dataQuery = dataQuery.OrderBy(x => x.Text);

            // EXECUTE
            var viewQuery = dataQuery
               .Select(x => new
               {
                  x.idAccount,
                  x.Text,
                  Type = x.TypeValue,
                  x.ClosingDay,
                  x.DueDay
               })
               .OrderBy(x => x.Type).ThenBy(x => x.Text)
               .AsQueryable();
            var viewData = await Task.FromResult(viewQuery.ToList());

            // CONVERT
            var dataResult = viewData
               .Select(x => new Base.viewRelated()
               {
                  ID = x.idAccount.ToString(),
                  textValue = x.Text,
                  htmlValue = "" +
                     "<span>" +
                        "<span>" + x.Text + "</span>" +
                        "<i class='material-icons left'>" + Model.viewAccount.GetIcon((Model.enAccountType)x.Type) + "</i>" +
                     "</span>",
                  jsonValue = FriendCash.Model.Base.Json.Serialize(x)
               })
               .ToList();
            return Ok(dataResult);

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }

      #endregion


      #region Create
      [Authorize(Roles = "ActiveUser")]
      [HttpPost, Route("")]
      public async Task<IHttpActionResult> Create(Model.viewAccount value)
      {
         try
         {

            // VALIDATE DUPLICITY
            if (this.QueryData().Count(x => x.Text == value.Text) != 0)
            { ModelState.AddModelError("Text", this.GetTranslation(Constants.WARNING_TEXT_DUPLICITY)); }

            // VALIDATE MODEL
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // CREATE
            var oData = new Model.bindAccount()
            {
               idUser = this.GetUserID(), 
               Text = value.Text,
               Type = value.Type,
               Active = value.Active,
               ClosingDay = value.ClosingDay,
               DueDay = value.DueDay,
               RowStatus = Base.BaseModel.enRowStatus.Active
            };

            // APPLY
            this.DataContext.Accounts.Add(oData);
            await this.DataContext.SaveChangesAsync();

            // RESULT
            var locationHeader = ""; // new Uri(Url.Link("GetAccountByID", new { id = oData.idAccount }));
            return Created(locationHeader, new Model.viewAccount(oData));

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region Update
      [Authorize(Roles = "ActiveUser")]
      [HttpPut, Route("")]
      public async Task<IHttpActionResult> Update(Model.viewAccount value)
      {
         try
         {

            // VALIDATE CREDIT CARD
            if (value.Type == Model.enAccountType.CreditCard)
            {
               if (!value.ClosingDay.HasValue || value.ClosingDay <= 0 || value.ClosingDay >= 31)
               { ModelState.AddModelError("ClosingDay", this.GetTranslation(Constants.WARNING_CLOSINGDAY_REQUIRED)); }
               if (!value.DueDay.HasValue || value.DueDay <= 0 || value.DueDay >= 31)
               { ModelState.AddModelError("DueDay", this.GetTranslation(Constants.WARNING_DUEDAY_REQUIRED)); }
            }

            // VALIDATE MODEL            
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // LOCATE
            var oData = await Task.FromResult(this.QueryData().Where(x => x.idAccount == value.idAccount).FirstOrDefault());
            if (oData == null) { ModelState.AddModelError("WarningMessage", this.GetTranslation(Constants.WARNING_ACCOUNT_NOTFOUND)); return BadRequest(ModelState); }

            // APPLY
            oData.Text = value.Text;
            oData.Type = value.Type;
            oData.ClosingDay = value.ClosingDay;
            oData.DueDay = value.DueDay;
            oData.Active = value.Active;
            await this.DataContext.SaveChangesAsync();

            // RESULT
            return Ok(new Model.viewAccount(oData));

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region Delete

      [Authorize(Roles = "ActiveUser")]
      [HttpDelete, Route("")]
      public async Task<IHttpActionResult> Delete(Model.viewAccount value)
      {
         return await this.Delete(value.idAccount);
      }

      [Authorize(Roles = "ActiveUser")]
      [HttpDelete, Route("{id:long}")]
      public async Task<IHttpActionResult> Delete(long id)
      {
         try
         {

            // VALIDATE MODEL
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // LOCATE
            var oData = await Task.FromResult(this.QueryData().Where(x => x.idAccount == id).FirstOrDefault());
            if (oData == null) { ModelState.AddModelError("WarningMessage", this.GetTranslation(Constants.WARNING_ACCOUNT_NOTFOUND)); return BadRequest(ModelState); }

            // APPLY
            oData.RowStatus = Base.BaseModel.enRowStatus.Removed;
            await this.DataContext.SaveChangesAsync();

            // RESULT
            return Ok();

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }

      #endregion

      #region RemoveUserData
      [Authorize(Roles = "Admin")]
      // [HttpDelete, Route("{id:long}")]
      internal async Task<bool> RemoveUserData(string idUser)
      {
         try
         {
            var iDelete = await this.DataContext.ObjectContext.ExecuteStoreCommandAsync(string.Format("delete from v5_dataAccounts where idUser='{0}'", idUser));
            return true;
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

   }

}
