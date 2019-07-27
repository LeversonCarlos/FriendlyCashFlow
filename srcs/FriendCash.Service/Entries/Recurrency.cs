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

   [RoutePrefix("api/recurrency")]
   public class RecurrencyController : Base.BaseController
   {

      #region Query

      internal IQueryable<Model.bindRecurrency> QueryData()
      {
         var idUser = this.GetUserID();
         return this.DataContext.Recurrencies
            .Where(x => x.RowStatusValue == (short)Base.BaseModel.enRowStatus.Active && x.idUser == idUser)
            .AsQueryable();
      }

      #endregion   

      #region Generate

      [Authorize(Roles = "ActiveUser")]
      [HttpPost, Route("generate")]
      public async Task<IHttpActionResult> Generate()
      {
         try
         {

            // RECURRENCY LIST
            var recurrencyQuery = this.QueryData()
               .Where(x => x.StateValue == (short)Model.enRecurrencyState.Active)
               .AsQueryable();
            var recurrencyList = await Task.FromResult(recurrencyQuery.ToList());
            if (recurrencyList == null || recurrencyList.Count == 0) { return Ok(); }

            // GENERATE
            foreach (var recurrencyModel in recurrencyList)
            {
               await this.Generate(recurrencyModel);
            }

            // RESULT 
            return Ok();

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }

      internal async Task<bool> Generate(Model.bindRecurrency value)
      {
         try
         {

            // QUANTITY TO GENERATE
            var totalQuantity = (value.Quantity - 1);
            if (!value.Fixed) { totalQuantity = 3; }

            // PATTERN
            Model.bindPattern patternModel = null;
            using (var patternController = new PatternController())
            { patternModel = await patternController.GetSingle(value.idPattern.Value); }

            // LOOP
            var generatedQuantity = 0;
            var entryDate = value.EntryDate;
            using (var entryController = new EntryController())
            {
               while (generatedQuantity < totalQuantity)
               {

                  // FREQUENCY
                  entryDate = this.NextDate(value.Type, entryDate);

                  // MODEL
                  var entryModel = new Model.viewEntry()
                  {
                     Text = patternModel.Text,
                     Type = patternModel.Type,
                     idCategory = patternModel.idCategory,
                     DueDate = entryDate,
                     EntryValue = Math.Abs(value.EntryValue),
                     idAccount = value.idAccount,
                     idPattern = value.idPattern,
                     idRecurrency = value.idRecurrency,
                     Paid = false
                  };
                  var entryResult = await entryController.Create(entryModel);
                  entryController.ModelState.Clear();

                  // NEXT
                  generatedQuantity++;
               }
            }

            // UPDATE RECURRENCY
            value.EntryDate = entryDate;           
            if (value.Fixed) { value.State = Model.enRecurrencyState.Inactive; }
            if (!value.Fixed) { value.Quantity += generatedQuantity; }
            await this.DataContext.SaveChangesAsync();

            // RESULT
            return true;

         }
         catch (Exception ex) { throw ex; }
      }

      #endregion

      #region Add

      internal async Task<Model.bindRecurrency> Add(Model.viewEntry value)
      {
         try
         {

            // RECURRENCY
            var idRecurrency = await this.Add_GetRecurrency(value);
            var recurrencyModel = this.QueryData().Where(x => x.idRecurrency == idRecurrency).FirstOrDefault();

            // RESULT
            return recurrencyModel;

         }
         catch { throw; }
      }

      private async Task<long> Add_GetRecurrency(Model.viewEntry value)
      {
         try
         {

            // ALREADY SET
            if (value.idRecurrency != null && value.idRecurrency.HasValue && value.idRecurrency.Value > 0)
            { return value.idRecurrency.Value; }

            // ANALYSE
            if (value.idRecurrencyView == null || value.idRecurrencyView.hasRecurrency == false) { return 0; }
            var idUser = this.GetUserID();

            // ADD NEW
            var recurrencyModel = new Model.bindRecurrency();
            recurrencyModel.idUser = idUser;
            recurrencyModel.idAccount = value.idAccount;
            recurrencyModel.idPattern = value.idPattern;
            recurrencyModel.InitialDate = value.DueDate;
            recurrencyModel.EntryDate = value.DueDate;
            recurrencyModel.EntryValue = value.EntryValue;
            recurrencyModel.Type = value.idRecurrencyView.Type;
            recurrencyModel.Fixed = value.idRecurrencyView.Fixed;
            recurrencyModel.Quantity = value.idRecurrencyView.Quantity;
            recurrencyModel.RowStatus = Base.BaseModel.enRowStatus.Active;
            recurrencyModel.State = Model.enRecurrencyState.Active;
            if (!recurrencyModel.Fixed) { recurrencyModel.Quantity = 1; }
            this.DataContext.Recurrencies.Add(recurrencyModel);
            await this.DataContext.SaveChangesAsync();
            if (recurrencyModel != null && recurrencyModel.idRecurrency != 0) { return recurrencyModel.idRecurrency; }

            // OTHERWISE
            throw new Exception("Entry recurrency could not be set");
         }
         catch { throw; }
      }

      #endregion

      #region Set
      internal async Task<Model.bindRecurrency> Set(Model.viewEntry value)
      {
         try
         {

            // LOCATE
            if (value.idRecurrency == null || !value.idRecurrency.HasValue || value.idRecurrency.Value == 0) { return null; }
            if (value.idRecurrencyView == null || value.idRecurrencyView.Update == Model.enRecurrencyUpdate.Current) { return null; }
            var recurrencyModel = this.QueryData().Where(x => x.idRecurrency == value.idRecurrency.Value).FirstOrDefault();
            if (recurrencyModel == null) { return null; }

            // APPLY CHANGES
            recurrencyModel.idAccount = value.idAccount;
            recurrencyModel.idPattern = value.idPattern;
            recurrencyModel.EntryValue = value.EntryValue;
            recurrencyModel.EntryDate = value.DueDate;

            // ENTRIES
            using (var entryController = new EntryController())
            {
               var entriesQuery = entryController.QueryView()
                  .Where(x => x.idRecurrency == value.idRecurrency && x.idEntry > value.idEntry);
               var entriesList = await Task.FromResult(entriesQuery.ToList());
               foreach (var entryModel in entriesList)
               {
                  recurrencyModel.EntryDate = this.NextDate(recurrencyModel.Type, recurrencyModel.EntryDate);
                  if (!entryModel.Paid) {
                     entryModel.idAccount = recurrencyModel.idAccount;
                     entryModel.DueDate = recurrencyModel.EntryDate;
                     entryModel.EntryValue = recurrencyModel.EntryValue;
                     entryModel.idPattern = recurrencyModel.idPattern;
                     entryModel.Text = value.Text;
                     await entryController.Update(entryModel);
                  }
               }
            }

            // UPDATE
            await this.DataContext.SaveChangesAsync();

            // RESULT
            return recurrencyModel;

         }
         catch { throw; }
      }
      #endregion

      #region NextDate
      private DateTime NextDate(Model.enRecurrencyType type, DateTime entryDate)
      {
         if (type == Model.enRecurrencyType.Weekly) { return entryDate.AddDays(7); }
         else if (type == Model.enRecurrencyType.Monthly) { return entryDate.AddMonths(1); }
         else if (type == Model.enRecurrencyType.Bimonthly) { return entryDate.AddMonths(2); }
         else if (type == Model.enRecurrencyType.Quarterly) { return entryDate.AddMonths(3); }
         else if (type == Model.enRecurrencyType.SemiYearly) { return entryDate.AddMonths(6); }
         else if (type == Model.enRecurrencyType.Yearly) { return entryDate.AddYears(1); }
         else { return entryDate; }
      }
      #endregion

      #region RemoveUserData
      [Authorize(Roles = "Admin")]
      // [HttpDelete, Route("{id:long}")]
      internal async Task<bool> RemoveUserData(string idUser)
      {
         try
         {
            var iDelete = await this.DataContext.ObjectContext.ExecuteStoreCommandAsync(string.Format("delete from v5_dataRecurrencies where idUser='{0}'", idUser));
            return true;
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

   }

   public class viewRecurrencyGenerate
   {
      public string idUser { get; set; }
   }

}
