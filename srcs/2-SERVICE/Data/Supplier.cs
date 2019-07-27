using System;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;
using FriendCash.Model.Tools;

namespace FriendCash.Service
{
   public class Supplier : Base
   {

      #region New
      internal Supplier(Model.Login oLogin) : base(oLogin) { }
      internal Supplier(Model.Context oContext) : base(oContext) { }
      #endregion

      #region Constants
      public const string TAG_ENTITY = "Supplier";
      public const string TAG_ENTITY_LIST = TAG_ENTITY + "s";
      public const string TAG_ENTITY_KEY = "id" + TAG_ENTITY;
      public const string TAG_ENTITY_CODE = "Code";
      public const string TAG_ENTITY_DESCRIPTION = "Description";
      #endregion

      #region GetQuery
      internal IQueryable<Model.Supplier> GetQuery(Parameters oParameters)
      {
         IQueryable<Model.Supplier> oQuery = null;

         // INITIAL
         oQuery = from DATA in this.Context.Suppliers
                  where DATA.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                  select DATA;
         this.ApplyLogin(ref oQuery, oParameters.Login);

         // ID
         long idRow = oParameters.GetNumericData(TAG_ID);
         if (idRow != 0) { oQuery = oQuery.Where(DATA => DATA.idRow == idRow); }

         // KEY
         long idKey = oParameters.GetNumericData(TAG_ENTITY_KEY);
         if (idKey != 0) { oQuery = oQuery.Where(DATA => DATA.idSupplier == idKey); }

         // CODE
         string Code = oParameters.GetTextData(TAG_ENTITY_CODE);
         if (! string.IsNullOrEmpty(Code)) { oQuery = oQuery.Where(DATA => DATA.Code == Code); }

         // DESCRIPTION
         string Description = oParameters.GetTextData(TAG_ENTITY_DESCRIPTION);
         if (!string.IsNullOrEmpty(Description)) { oQuery = oQuery.Where(DATA => DATA.Description == Description); }

         // SEARCH
         string sSearch = oParameters.GetTextData(TAG_SEARCH);
         if (!string.IsNullOrEmpty(sSearch))
         {
            oQuery = oQuery.Where(DATA => DATA.Code.Contains(sSearch) || DATA.Description.Contains(sSearch));
          }

         // ORDER BY
         oQuery = oQuery.OrderBy(DATA => DATA.Description);

         return oQuery;
      }
      #endregion

      #region GetData

      #region GetData
      private Return GetData(Parameters oParameters, bool bApplyPagination)
      {
         var oReturn = new Return();

         try
         {

            // QUERY
            var oQuery = this.GetQuery(oParameters);

            // PAGINATION
            if (bApplyPagination == true)
            { this.ApplyPagination<Service.Supplier, Model.Supplier>(ref oQuery, oParameters, ref oReturn); }

            // EXECUTE
            oReturn.DATA.Add(TAG_ENTITY_LIST, oQuery.ToList<Model.Supplier>());
            oReturn.DATA.Add(TAG_SEARCH, oParameters.GetTextData(TAG_SEARCH));

            // OK
            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
       }
      #endregion

      #region GetDataSingle
      internal Model.Supplier GetDataSingle(Model.Login oLogin, long idRow, long idKey, string Code)
      {
         Model.Supplier oReturn = null;

         try
         {

            // PARAMETERS
            var oParameters = new Parameters();
            oParameters.DATA.Add(TAG_ID, idRow);
            oParameters.DATA.Add(TAG_ENTITY_KEY, idKey);
            oParameters.DATA.Add(TAG_ENTITY_CODE, Code);
            if (oLogin != null) { oParameters.Login = oLogin; }

            // EXECUTE
            var oExecReturn = this.GetData(oParameters, false);

            // RESULT
            if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oExecReturn.DATA[TAG_ENTITY_LIST] != null)
            {
               var oList = ((List<Model.Supplier>)oExecReturn.DATA[TAG_ENTITY_LIST]);
               oReturn = oList.SingleOrDefault();
             }

         }
         catch { throw; }

         return oReturn;
      }
      #endregion

      #region GetDataList
      internal List<Model.Supplier> GetDataList(Model.Login oLogin, string Search)
      {
         List<Model.Supplier> oReturn = null;

         // PARAMETERS
         var oParameters = new Parameters();
         oParameters.DATA.Add(TAG_SEARCH, Search);
         oParameters.Login = oLogin;

         // EXECUTE
         var oExecReturn = this.GetData(oParameters, false);

         // RESULT
         if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oExecReturn.DATA[TAG_ENTITY_LIST] != null)
         {
            oReturn = ((List<Model.Supplier>)oExecReturn.DATA[TAG_ENTITY_LIST]);
         }

         return oReturn;
      }
      #endregion

      #endregion

      #region Executes

      #region Index
      public static Return Index(Parameters oParameters)
      {
         Return oReturn = null;

         try
         {

            // INSTANCE
            using (var oService = new Service.Supplier(oParameters.Login))
            {

               // EXECUTE
               oReturn = oService.GetData(oParameters, true);

            }

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }
      #endregion

      #region Create
      public static Return Create(Parameters oParameters)
      {
         var oReturn = new Return();

         try
         {

            // EMPTY MODEL
            oReturn.DATA.Add(TAG_ENTITY, new Model.Supplier());

            // OK
            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }
      #endregion

      #region Edit
      public static Return Edit(Parameters oParameters)
      {
         Return oReturn = null;

         try
         {

            // INSTANCE
            using (var oService = new Service.Supplier(oParameters.Login))
            {

               // EXECUTE
               oReturn = oService.GetData(oParameters, false);

               // RESULT
               if (oReturn.OK == true && oReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oReturn.DATA[TAG_ENTITY_LIST] != null)
               {
                  var oList = ((List<Model.Supplier>)oReturn.DATA[TAG_ENTITY_LIST]);
                  if (oList.Count == 0) { oReturn.MSG.Add(new Message() { Warning = Resources.Suppliers.MSG_WARNING_NOT_FOUND }); oReturn.OK = false; }
                  if (oList.Count >= 2) { oReturn.MSG.Add(new Message() { Warning = Resources.Suppliers.MSG_WARNING_DUPLICITY }); oReturn.OK = false; }
                  else { oReturn.DATA.Add(TAG_ENTITY, oList.SingleOrDefault()); }
               }

            }

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }
      #endregion

      #region SaveEdit

      public static Return SaveEdit(Parameters oParameters)
      {
         var oReturn = new Return();

         try
         {

            // INSTANCE
            using (var oService = new Service.Supplier(oParameters.Login))
            {

               // VALIDATE
               if (oService.SaveEdit_Validate(oReturn, oParameters) == false) { return oReturn; }

               // APPLY
               if (oService.SaveEdit_Apply(oReturn, oParameters) == false) { return oReturn; }

               // OK
               oReturn.OK = true;

            }

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }

      private bool SaveEdit_Validate(Return oReturn, Parameters oParameters)
      {

         // DATA
         var oValue = ((Model.Supplier)oParameters.DATA[TAG_ENTITY]);

         // CODE
         if (oValue.Code == null || string.IsNullOrEmpty(oValue.Code))
         { oReturn.MSG.Add(new Message() { Warning = Resources.Suppliers.MSG_REQUIRED_CODE }); return false; }

         // CHECK CODE DUPLICITY
         if (this.ModelSet<Model.Supplier>(oParameters.Login).Where(DATA => 
                  DATA.Code == oValue.Code &&
                  DATA.RowStatusValue == (short)Model.Base.enRowStatus.Active &&
                  DATA.idSupplier != oValue.idSupplier).Count() != 0)
         { oReturn.MSG.Add(new Message() { Warning = Resources.Suppliers.MSG_WARNING_DUPLICITY }); return false; }

         // DESCRIPTION
         if (oValue.Description == null || string.IsNullOrEmpty(oValue.Description))
         { oReturn.MSG.Add(new Message() { Warning = Resources.Suppliers.MSG_REQUIRED_DESCRIPTION }); return false; }

         // CHECK DESCRIPTION DUPLICITY
         if (this.ModelSet<Model.Supplier>(oParameters.Login).Where(DATA => 
                  DATA.Description == oValue.Description &&
                  DATA.RowStatusValue == (short)Model.Base.enRowStatus.Active &&
                  DATA.idSupplier != oValue.idSupplier).Count() != 0)
         { oReturn.MSG.Add(new Message() { Warning = Resources.Suppliers.MSG_WARNING_DUPLICITY }); return false; }

         return true;
      }

      private bool SaveEdit_Apply(Return oReturn, Parameters oParameters)
      {
         bool bReturn = false;

         try
         {

            // DATA
            var oData = ((Model.Supplier)oParameters.DATA[TAG_ENTITY]);

            // APPLY
            var oAfterSave = new Action<Model.Supplier>(DATA => { if (DATA.idSupplier == 0) { DATA.idSupplier = DATA.idRow; } });
            if (this.ApplySave(oData, oParameters.Login, oReturn.MSG, oAfterSave) == false) { return bReturn; }

            // OK
            bReturn = true;

         }
         catch { throw; }

         return bReturn;
      }

      #endregion

      #region SaveRemove

      public static Return SaveRemove(Parameters oParameters)
      {
         var oReturn = new Return();

         try
         {

            // INSTANCE
            using (var oService = new Service.Supplier(oParameters.Login))
            {

               // VALIDATE
               if (oService.SaveRemove_Validate(oReturn, oParameters) == false) { return oReturn; }

               // APPLY
               if (oService.SaveRemove_Apply(oReturn, oParameters) == false) { return oReturn; }

               // OK
               oReturn.OK = true;

            }

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }

      private bool SaveRemove_Validate(Return oReturn, Parameters oParameters)
      {
         bool bReturn = false;

         try
         {

            // CHECK DATA
            if (oParameters.DATA.ContainsKey(TAG_ENTITY) == false)
            {
               oReturn.MSG.Add(new Message() { Warning = Resources.Suppliers.MSG_WARNING_NOT_FOUND });
               return bReturn;
            }

            // CHECK EXISTENCE
            var oDATA = ((Model.Supplier)oParameters.DATA[TAG_ENTITY]);
            if (this.Context.Suppliers.Where(DATA => DATA.idRow == oDATA.idRow).Count() == 0)
            {
               oReturn.MSG.Add(new Message() { Warning = Resources.Suppliers.MSG_WARNING_NOT_FOUND });
               return bReturn;
            }

            // CHECK FOREIGN KEYS
            if (this.Context.Documents.Where(DATA => DATA.idSupplier == oDATA.idRow && DATA.RowStatusValue == (short)Model.Base.enRowStatus.Active).Count() != 0) 
            {
               oReturn.MSG.Add(new Message() { Warning = Resources.Suppliers.MSG_WARNING_DATA_BEING_RELATED });
               return bReturn;
            }

            // OK
            bReturn = true;

         }
         catch { throw; }

         return bReturn;
      }

      private bool SaveRemove_Apply(Return oReturn, Parameters oParameters)
      {
         bool bReturn = false;

         try
         {

            // PARAMETER
            var oData = ((Model.Supplier)oParameters.DATA[TAG_ENTITY]);

            // APPLY
            if (this.ApplyRemove(oData, oParameters.Login, oReturn.MSG) == false) { return bReturn; }

            // RETURN 
            oReturn.DATA.Add(TAG_ENTITY, oData);

            // OK
            bReturn = true;

         }
         catch { throw; }

         return bReturn;
      }

      #endregion

      #region AutoComplete
      public static Return AutoComplete(Parameters oParameters)
      {
         var oReturn = new Return();

         try
         {

            // INSTANCE
            using (var oService = new Service.Supplier(oParameters.Login))
            {

               // QUERY
               var oList = oService.GetDataList(oParameters.Login, oParameters.GetTextData(TAG_SEARCH));

               // QUERY: AUTO COMPLETE
               var oResultQuery =
                  from DATA in oList
                  select new AutoCompleteData
                  {
                     Value = DATA.idSupplier.ToString(),
                     Description = DATA.Description,
                     Details =
                        "<div class='row'>" +
                           "<span class='hidden-xs col-sm-3'>" + DATA.Code + "</span>" +
                           "<span class='col-sm-9'>" + DATA.Description + "</span>" +
                        "</div>"
                  };

               // EXECUTE
               var oResultData = oResultQuery.ToList<AutoCompleteData>();
               oReturn.DATA.Add(AutoCompleteData.TAG_LIST_NAME, oResultData);

               // OK
               oReturn.OK = true;

            }

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }
      #endregion

      #endregion

   }
 }
