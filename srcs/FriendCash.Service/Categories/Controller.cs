#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;
#endregion

namespace FriendCash.Service.Categories
{

   [RoutePrefix("api/categories")]
   public class CategoryController : Base.BaseController
   {

      #region Contants
      internal class Constants
      {
         internal const string WARNING_TEXT_DUPLICITY = "MSG_CATEGORIES_TEXT_DUPLICITY";
         internal const string WARNING_CATEGORY_NOTFOUND = "MSG_CATEGORIES_CATEGORY_NOTFOUND";
      }
      #endregion

      #region Query

      internal IQueryable<Model.bindCategory> QueryData()
      {
         var idUser = this.GetUserID();
         return this.DataContext.Categories
            .Where(x => x.RowStatusValue == (short)Base.BaseModel.enRowStatus.Active && x.idUser == idUser)
            .AsQueryable();
      }

      internal IQueryable<Model.viewCategory> QueryView()
      {
         return this.QueryData()
            .Select(x => new Model.viewCategory()
            {
               idCategory = x.idCategory,
               Text = x.Text,
               HierarchyText = x.HierarchyText,
               TypeValue = x.TypeValue,
               idParentRow = x.idParentRow
            })
            .AsQueryable();
      }

      #endregion


      #region GetAll

      [Authorize(Roles = "User,Viewer")]
      [Route("{type:alpha}")]
      public async Task<IHttpActionResult> GetAll(string type)
      {
         try
         {
            var iType = (Model.enCategoryType)Enum.Parse(typeof(Model.enCategoryType), type);
            var oQuery = this.QueryView().Where(x=> x.TypeValue == (short)iType);
            var oCategories = await Task.FromResult(oQuery.ToList());
            var oData = this.GetAll(oCategories, 0);
            return Ok(oData);

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }

      private List<Model.viewCategory> GetAll(List<Model.viewCategory> oCategories, long? idParentRow)
      {
         return oCategories.Where(x => x.idParentRow == idParentRow.Value || (idParentRow == 0 && x.idParentRow == null))
            .Select(x => new Model.viewCategory()
            {
               idCategory = x.idCategory,
               HierarchyText = x.HierarchyText,
               Text = x.Text,
               Type = x.Type,
               idParentRow = x.idParentRow,
               Childs = this.GetAll(oCategories, x.idCategory)
            })
            .OrderBy(x => x.HierarchyText)
            .ToList();
      }

      #endregion

      #region GetSingle
      [Authorize(Roles = "User,Viewer")]
      [Route("{id:long}", Name = "GetCategoryByID")]
      public async Task<IHttpActionResult> GetSingle(long id)
      {
         try
         {
            var oQuery = this.QueryView().Where(x => x.idCategory == id);
            var oData = await Task.FromResult(oQuery.FirstOrDefault());
            return Ok(oData);

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region GetRelated

      [Authorize(Roles = "User,Viewer")]
      [Route("{type:alpha}/related/{searchID:long}")]
      [Route("{type:alpha}/related/{searchText}/")]
      [Route("{type:alpha}/related")]
      public async Task<IHttpActionResult> GetRelatedInner(string type, long searchID = 0, string searchText = "")
      {
         try
         {

            // MAIN QUERY
            var dataType = (Model.enCategoryType)Enum.Parse(typeof(Model.enCategoryType), type);
            var dataQuery = this.QueryData()
               .Where(x => x.TypeValue == (short)dataType)
               .AsQueryable();

            // FILTER: ID
            if (searchID > 0)
            { dataQuery = dataQuery.Where(x => x.idCategory == searchID); }

            // FILTER: TEXT
            if (!string.IsNullOrEmpty(searchText))
            { dataQuery = dataQuery.Where(x => x.HierarchyText.Contains(searchText)); }

            // ORDER
            dataQuery = dataQuery.OrderBy(x => x.HierarchyText);

            // RESULT
            var viewQuery = dataQuery
               .Select(x => new Base.viewRelated()
               {
                  ID = x.idCategory.ToString(),
                  textValue = x.HierarchyText,
                  htmlValue = "<span>" + x.HierarchyText + "</span>",
                  jsonValue = x.HierarchyText
               })
               .AsQueryable();

            // EXECUTE
            var dataResult = await Task.FromResult(viewQuery.ToList());
            return Ok(dataResult);
         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }

      #endregion


      #region Create
      [Authorize(Roles = "ActiveUser")]
      [HttpPost, Route("")]
      public async Task<IHttpActionResult> Create(Model.viewCategory value)
      {
         try
         {

            // VALIDATE MODEL
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // VALIDATE DUPLICITY
            if (this.QueryData().Count(x => x.TypeValue == (short)value.Type && x.idParentRow == value.idParentRow && x.Text == value.Text) != 0)
            { ModelState.AddModelError("Text", this.GetTranslation(Constants.WARNING_TEXT_DUPLICITY)); return BadRequest(ModelState); }

            // CREATE
            var oData = new Model.bindCategory()
            {
               idUser = this.GetUserID(), 
               Text = value.Text,
               Type = value.Type,
               idParentRow = value.idParentRow,
               RowStatus = Base.BaseModel.enRowStatus.Active
            };

            // HIERARCHY TEXT
            var hierarchyText = value.Text;
            if (value.idParentRow != null && value.idParentRow.HasValue && value.idParentRow.Value > 0)
            {
               var parentText = this.QueryData()
                  .Where(x => x.idCategory == value.idParentRow.Value)
                  .Select(x => x.HierarchyText)
                  .FirstOrDefault();
               if (!string.IsNullOrEmpty(parentText)) {
                  hierarchyText = parentText + " \\ " + hierarchyText;
               }
             }
            oData.HierarchyText = hierarchyText;

            // APPLY
            this.DataContext.Categories.Add(oData);
            await this.DataContext.SaveChangesAsync();

            // RESULT
            var locationHeader = ""; // new Uri(Url.Link("GetCategoryByID", new { id = oData.idCategory }));
            return Created(locationHeader, new Model.viewCategory(oData));

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region Update
      [Authorize(Roles = "ActiveUser")]
      [HttpPut, Route("")]
      public async Task<IHttpActionResult> Update(Model.viewCategory value)
      {
         try
         {

            // VALIDATE MODEL
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // LOCATE
            var oData = await Task.FromResult(this.QueryData().Where(x => x.idCategory == value.idCategory).FirstOrDefault());
            if (oData == null) { ModelState.AddModelError("WarningMessage", this.GetTranslation(Constants.WARNING_CATEGORY_NOTFOUND)); return BadRequest(ModelState); }

            // HIERARCHY TEXT
            var hierarchyText = value.Text;
            if (value.idParentRow != null && value.idParentRow.HasValue && value.idParentRow.Value > 0)
            {
               var parentText = this.QueryData()
                  .Where(x => x.idCategory == value.idParentRow.Value)
                  .Select(x => x.HierarchyText)
                  .FirstOrDefault();
               if (!string.IsNullOrEmpty(parentText))
               {
                  hierarchyText = parentText + " \\ " + hierarchyText;
               }
            }
            oData.HierarchyText = hierarchyText;

            // APPLY
            oData.Text = value.Text;
            oData.idParentRow = value.idParentRow;
            await this.DataContext.SaveChangesAsync();

            // RESULT
            return Ok(new Model.viewCategory(oData));

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region Delete

      [Authorize(Roles = "ActiveUser")]
      [HttpDelete, Route("")]
      public async Task<IHttpActionResult> Delete(Model.viewCategory value)
      {
         return await this.Delete(value.idCategory);
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
            var oData = await Task.FromResult(this.QueryData().Where(x => x.idCategory == id).FirstOrDefault());
            if (oData == null) { ModelState.AddModelError("WarningMessage", this.GetTranslation(Constants.WARNING_CATEGORY_NOTFOUND)); return BadRequest(ModelState); }

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
            var iDelete = await this.DataContext.ObjectContext.ExecuteStoreCommandAsync(string.Format("delete from v5_dataCategories where idUser='{0}'", idUser));
            return true;
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

   }

}
