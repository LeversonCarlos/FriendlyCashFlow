#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;
#endregion

namespace FriendCash.Service.Balances
{

   [RoutePrefix("api/balances")]
   public class BalanceController : Base.BaseController
   {

      #region Query

      internal IQueryable<Model.dataBalance> QueryData()
      {
         var idUser = this.GetUserID();
         return this.DataContext.Balances
            .Where(x => x.RowStatusValue == (short)Base.BaseModel.enRowStatus.Active && x.idUser == idUser)
            .AsQueryable();
      }

      #endregion


      #region Get
      internal async Task<Model.dataBalance> Get(int year, int month, long account)
      {
         try
         {

            // WHERE
            var searchDate = new DateTime(year, month, 1);
            var dataWhere = this.QueryData().Where(x => x.SearchDate < searchDate);
            if (account > 0) { dataWhere = dataWhere.Where(x=> x.idAccount == account); }

            // SELECT 
            var dataQuery = dataWhere
               .GroupBy(x => x.idUser)
               .Select(x => new 
               {
                  idUser = x.Key,
                  SearchDate = searchDate,
                  idAccount = account,
                  PaidValue = x.Sum(g => g.PaidValue),
                  TotalValue = x.Sum(g => g.TotalValue),
                  RowStatus = Base.BaseModel.enRowStatus.Active
               });

            // EXECUTE
            var dataTemp = await Task.FromResult(dataQuery.FirstOrDefault());

            // VALIDATE
            if (dataTemp == null)
            {
               dataTemp = new 
               {
                  idUser = this.GetUserID(),
                  SearchDate = searchDate,
                  idAccount = account,
                  PaidValue = (double)0,
                  TotalValue = (double)0,
                  RowStatus = Base.BaseModel.enRowStatus.Active
               };
            }
            
            // RETURN
            var dataJson = FriendCash.Model.Base.Json.Serialize(dataTemp);
            var dataModel = FriendCash.Model.Base.Json.Deserialize<Model.dataBalance>(dataJson);
            return dataModel;

         }
         catch (Exception ex)  { throw ex; }
      }
      #endregion


      #region Add
      internal async Task<IHttpActionResult> Add(Entries.Model.bindEntry value)
      {
         try
         {

            // VALIDATE ACCOUNT
            if (value.idAccount.HasValue == false || value.idAccount.Value == 0) { return Ok(); }

            // LOCATE
            var oQuery = this.QueryData()
               .Where(x => x.SearchDate.Year == value.SearchDate.Year &&
                          x.SearchDate.Month == value.SearchDate.Month &&
                          x.idAccount == value.idAccount);
            var oData = await Task.FromResult(oQuery.FirstOrDefault());

            // CREATE
            if (oData == null)
            {
               oData = new Model.dataBalance()
               {
                  idUser = this.GetUserID(),
                  SearchDate = new DateTime(value.SearchDate.Year, value.SearchDate.Month, 1),
                  idAccount = value.idAccount.Value, 
                  RowStatus = Base.BaseModel.enRowStatus.Active
               };
               this.DataContext.Balances.Add(oData);
            }

            // VALUE
            var iValue = value.Value;
            if (value.Type == Entries.Model.enEntryType.Expense) { iValue = iValue * -1; }

            // APPLY
            oData.TotalValue += iValue;
            if (value.Paid) { oData.PaidValue += iValue; }
            await this.DataContext.SaveChangesAsync();

            // RESULT
            return Ok();

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region Remove
      internal async Task<IHttpActionResult> Remove(Entries.Model.bindEntry value)
      {
         try
         {

            // VALIDATE ACCOUNT
            if (value.idAccount.HasValue == false || value.idAccount.Value == 0) { return Ok(); }

            // LOCATE
            var oQuery = this.QueryData()
               .Where(x => x.SearchDate.Year == value.SearchDate.Year &&
                          x.SearchDate.Month == value.SearchDate.Month &&
                          x.idAccount == value.idAccount);
            var oData = await Task.FromResult(oQuery.FirstOrDefault());
            if (oData == null) { return Ok(); }

            // VALUE
            var iValue = value.Value;
            if (value.Type == Entries.Model.enEntryType.Expense) { iValue = iValue * -1; }

            // APPLY
            oData.TotalValue -= iValue;
            if (value.Paid) { oData.PaidValue -= iValue; }
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
            var iDelete = await this.DataContext.ObjectContext.ExecuteStoreCommandAsync(string.Format("delete from v5_dataBalance where idUser='{0}'", idUser));
            return true;
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

   }

}
