#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;
#endregion

namespace FriendCash.Service.Entries
{

   [RoutePrefix("api/pattern")]
   public class PatternController : Base.BaseController
   {

      #region Query

      internal IQueryable<Model.bindPattern> QueryData()
      {
         var idUser = this.GetUserID();
         return this.DataContext.Patterns
            .Where(x => x.RowStatusValue == (short)Base.BaseModel.enRowStatus.Active && x.idUser == idUser)
            .AsQueryable();
      }

      #endregion

      #region GetRelated

      [Authorize(Roles = "User,Viewer")]
      [Route("{type:alpha}/related/{searchID:long}")]
      [Route("{type:alpha}/related/{searchText}/")]
      [Route("{type:alpha}/related")]
      public async Task<IHttpActionResult> GetRelated(string type, long searchID = 0, string searchText = "") 
      {
         try
         {

            // MAIN QUERY
            var dataType = (Model.enEntryType)Enum.Parse(typeof(Model.enEntryType), type);
            var dataQuery = this.QueryData()
                           .Where(x => x.TypeValue == (short)dataType)
               .AsQueryable();

            // FILTER: ID
            if (searchID > 0)
            { dataQuery = dataQuery.Where(x => x.idPattern == searchID); }

            // FILTER: TEXT
            else if (!string.IsNullOrEmpty(searchText))
            { dataQuery = dataQuery.Where(x => x.Text.Contains(searchText)); }

            // FILTER: NONE
            else { dataQuery = dataQuery.Where(x => x.idPattern < 0); }

            // EXECUTE
            var viewQuery = dataQuery
               .Select(x => new
               {
                  x.idPattern,
                  x.Text,
                  x.Quantity, 
                  x.idCategory
               })
               .OrderByDescending(x => x.Quantity)
               .Take(15);
            var viewData = await Task.FromResult(viewQuery.ToList());

            // CONVERT
            var dataResult = viewData
               .Select(x => new Base.viewRelated()
               {
                  ID = x.idPattern.ToString(),
                  textValue = x.Text,
                  htmlValue =
                     "<span>" + x.Text +
                        "<span class='badge'>" + x.Quantity.ToString() + "</span>" +
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

      #region GetSingle
      internal async Task<Model.bindPattern> GetSingle(long id)
      {
         try
         {
            var dataQuery = this.QueryData().Where(x => x.idPattern == id);
            var oData = await Task.FromResult(dataQuery.FirstOrDefault());
            return oData;
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion     



      #region Add

      internal async Task<Model.bindPattern> Add(Model.viewEntry value)
      {
         try
         {

            // PATTERN
            var idPattern = await this.Add_GetPattern(value);
            var patternModel = this.QueryData().Where(x => x.idPattern == idPattern).FirstOrDefault();
            if (patternModel == null) { return null; }

            // QUANTITY
            patternModel.Quantity += 1;
            await this.DataContext.SaveChangesAsync();

            // RESULT
            return patternModel;

         }
         catch { throw; }
      }

      private async Task<long> Add_GetPattern(Model.viewEntry value)
      {
         try
         {

            // ALREADY SET
            if (value.idPattern != null && value.idPattern.HasValue && value.idPattern.Value > 0)
            { return value.idPattern.Value; }

            // PATTERN HASH
            var patternModel = Model.bindPattern.Create(value);
            var patternHash = patternModel.GetHash();

            // SEARCH
            var idUser = this.GetUserID();
            patternModel = await Task.FromResult(this.QueryData().Where(x => x.Hash == patternHash).FirstOrDefault());
            if (patternModel != null && patternModel.idPattern != 0)
            { return patternModel.idPattern; }

            // ADD NEW
            patternModel = Model.bindPattern.Create(value);
            patternModel.idUser = idUser;
            patternModel.Hash = patternHash;
            patternModel.RowStatus = Base.BaseModel.enRowStatus.Active;
            this.DataContext.Patterns.Add(patternModel);
            await this.DataContext.SaveChangesAsync();
            if (patternModel != null && patternModel.idPattern != 0) { return patternModel.idPattern; }

            // OTHERWISE
            throw new Exception("Entry pattern could not be set");
         }
         catch { throw; }
      }

      #endregion

      #region Remove
      internal async Task<Model.bindPattern> Remove(long idPattern)
      {
         try
         {

            // PATTERN
            var patternModel = this.QueryData().Where(x => x.idPattern == idPattern).FirstOrDefault();
            if (patternModel == null) { return null; }

            // QUANTITY
            patternModel.Quantity -= 1;
            if (patternModel.Quantity < 0) { patternModel.Quantity = 0; }
            if (patternModel.Quantity == 0) { patternModel.RowStatusValue = (short)Base.BaseModel.enRowStatus.Removed; }
            await this.DataContext.SaveChangesAsync();

            // RESULT
            return patternModel;

         }
         catch { throw; }
      }
      #endregion

      #region RemoveUserData
      [Authorize(Roles = "Admin")]
      // [HttpDelete, Route("{id:long}")]
      internal async Task<bool> RemoveUserData(string idUser)
      {
         try
         {
            var iDelete = await this.DataContext.ObjectContext.ExecuteStoreCommandAsync(string.Format("delete from v5_dataPatterns where idUser='{0}'", idUser));
            return true;
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

   }

}
