using System;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;
using FriendCash.Model.Tools;

namespace FriendCash.Service
{
   public class Account : Base
   {

      #region New
      internal Account(Model.Login oLogin) : base(oLogin) { }
      internal Account(Model.Context oContext) : base(oContext) { }
      #endregion

      #region Constants
      public const string TAG_ENTITY = "Account";
      public const string TAG_ENTITY_LIST = TAG_ENTITY + "s";
      public const string TAG_ENTITY_KEY = "id" + TAG_ENTITY;
      public const string TAG_ENTITY_CODE = "Code";
      public const string TAG_ENTITY_DESCRIPTION = "Description";
      #endregion

      #region GetQuery
      internal IQueryable<Model.Account> GetQuery(Model.Login oLogin, long idRow, long idKey, string Code, string Description, string Search, bool JustEnabled)
      {
         IQueryable<Model.Account> oQuery = null;

         // INITIAL
         oQuery = from DATA in this.Context.Accounts
                  where DATA.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                  select DATA;
         this.ApplyLogin(ref oQuery, oLogin);

         // ID
         if (idRow != 0) { oQuery = oQuery.Where(DATA => DATA.idRow == idRow); }

         // KEY
         if (idKey != 0) { oQuery = oQuery.Where(DATA => DATA.idAccount == idKey); }

         // CODE
         if (!string.IsNullOrEmpty(Code)) { oQuery = oQuery.Where(DATA => DATA.Code == Code); }

         // DESCRIPTION
         if (!string.IsNullOrEmpty(Description)) { oQuery = oQuery.Where(DATA => DATA.Description == Description); }

         // SEARCH
         if (!string.IsNullOrEmpty(Search))
         {
            oQuery = oQuery.Where(DATA => DATA.Code.Contains(Search) || DATA.Description.Contains(Search) );
          }

         // JUST ENABLED
         if (JustEnabled == true) { oQuery = oQuery.Where(DATA => DATA.StatusValue == ((short)Model.Account.enStatus.Enabled)); }

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

            // PARAMETERS
            long idRow = oParameters.GetNumericData(TAG_ID);
            long Key = oParameters.GetNumericData(TAG_ENTITY_KEY);
            string Code = oParameters.GetTextData(TAG_ENTITY_CODE);
            string Description = oParameters.GetTextData(TAG_ENTITY_DESCRIPTION);
            string Search = oParameters.GetTextData(TAG_SEARCH);
            bool JustEnabled = (oParameters.DATA.ContainsKey("JustEnabled") && ((bool)oParameters.DATA["JustEnabled"]) == true);

            // QUERY
            var oQuery = this.GetQuery(oParameters.Login, idRow, Key, Code, Description, Search, JustEnabled);

            // PAGINATION
            if (bApplyPagination == true)
            { this.ApplyPagination<Service.Account, Model.Account>(ref oQuery, oParameters, ref oReturn); }

            // EXECUTE
            oReturn.DATA.Add(TAG_ENTITY_LIST, oQuery.ToList<Model.Account>());
            oReturn.DATA.Add(TAG_SEARCH, Search);

            // OK
            oReturn.OK = true;

          }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
       }
      #endregion

      #region GetDataSingle
      internal Model.Account GetDataSingle(Model.Login oLogin, long idRow, long idKey, string Code)
      {
         Model.Account oReturn = null;

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
               var oList = ((List<Model.Account>)oExecReturn.DATA[TAG_ENTITY_LIST]);
               oReturn = oList.SingleOrDefault();
             }

         }
         catch { throw; }

         return oReturn;
      }
      #endregion

      #region GetDataList
      internal List<Model.Account> GetDataList(Model.Login oLogin, string Search)
      {
         List<Model.Account> oReturn = null;

         // PARAMETERS
         var oParameters = new Parameters();
         oParameters.DATA.Add(TAG_SEARCH, Search);
         oParameters.Login = oLogin;

         // EXECUTE
         var oExecReturn = this.GetData(oParameters, false);

         // RESULT
         if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oExecReturn.DATA[TAG_ENTITY_LIST] != null)
         {
            oReturn = ((List<Model.Account>)oExecReturn.DATA[TAG_ENTITY_LIST]);
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
            using (var oService = new Service.Account(oParameters.Login))
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
            oReturn.DATA.Add(TAG_ENTITY, new Model.Account()); 

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
            using (var oService = new Service.Account(oParameters.Login))
            {

               // EXECUTE
               oReturn = oService.GetData(oParameters, false);

               // RESULT
               if (oReturn.OK == true && oReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oReturn.DATA[TAG_ENTITY_LIST] != null)
               {
                  var oList = ((List<Model.Account>)oReturn.DATA[TAG_ENTITY_LIST]);
                  if (oList.Count == 0) { oReturn.MSG.Add(new Message() { Warning = FriendCash.Resources.Accounts.MSG_WARNING_NOT_FOUND }); oReturn.OK = false; }
                  if (oList.Count >= 2) { oReturn.MSG.Add(new Message() { Warning = FriendCash.Resources.Accounts.MSG_WARNING_DUPLICITY }); oReturn.OK = false; }
                  else { oReturn.DATA.Add(TAG_ENTITY, oList.SingleOrDefault()); }
                  oReturn.DATA.Remove(TAG_ENTITY_LIST);
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
            using (var oService = new Service.Account(oParameters.Login))
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
         var oValue = ((Model.Account)oParameters.DATA[TAG_ENTITY]);

         // CODE
         if (oValue.Code == null || string.IsNullOrEmpty(oValue.Code))
         { oReturn.MSG.Add(new Message() { Warning = Resources.Accounts.MSG_REQUIRED_CODE }); return false; }

         // CHECK CODE DUPLICITY
         if (this.ModelSet<Model.Account>(oParameters.Login).Where(DATA => 
                  DATA.Code == oValue.Code &&
                  DATA.RowStatusValue == (short)Model.Base.enRowStatus.Active &&
                  DATA.idAccount != oValue.idAccount).Count() != 0)
         { oReturn.MSG.Add(new Message() { Warning = Resources.Suppliers.MSG_WARNING_DUPLICITY }); return false; }

         // DESCRIPTION
         if (oValue.Description == null || string.IsNullOrEmpty(oValue.Description))
         { oReturn.MSG.Add(new Message() { Warning = Resources.Accounts.MSG_REQUIRED_DESCRIPTION }); return false; }

         // CHECK DESCRIPTION DUPLICITY
         if (this.ModelSet<Model.Account>(oParameters.Login).Where(DATA => 
                  DATA.Description == oValue.Description &&
                  DATA.RowStatusValue == (short)Model.Base.enRowStatus.Active &&
                  DATA.idAccount != oValue.idAccount).Count() != 0)
         { oReturn.MSG.Add(new Message() { Warning = Resources.Suppliers.MSG_WARNING_DUPLICITY }); return false; }

         // TYPE
         if (
             oValue.Type == Model.Account.enType.CreditCard &&
             (oValue.DueDay == null || oValue.DueDay.HasValue == false || oValue.DueDay.Value <= 0 || oValue.DueDay.Value > 31)
            )
         { oReturn.MSG.Add(new Message() { Warning = Resources.Accounts.MSG_REQUIRED_DUE_DAY }); return false; }

         return true;
      }

      private bool SaveEdit_Apply(Return oReturn, Parameters oParameters)
      {
         bool bReturn = false;

         try
         {

            // DATA
            var oData = ((Model.Account)oParameters.DATA[TAG_ENTITY]);

            // APPLY
            var oAfterSave = new Action<Model.Account>(DATA => { if (DATA.idAccount == 0) { DATA.idAccount = DATA.idRow; } });
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
            using (var oService = new Service.Account(oParameters.Login))
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
               oReturn.MSG.Add(new Message() { Warning = FriendCash.Resources.Accounts.MSG_WARNING_NOT_FOUND }); 
               return bReturn;
            }

            // CHECK EXISTENCE
            var oDATA = ((Model.Account)oParameters.DATA[TAG_ENTITY]);
            if (this.Context.Accounts.Where(DATA => DATA.idRow == oDATA.idRow).Count() == 0)
            {
               oReturn.MSG.Add(new Message() { Warning = FriendCash.Resources.Accounts.MSG_WARNING_NOT_FOUND }); 
               return bReturn;
            }

            // CHECK FOREIGN KEYS
            if (this.Context.Historys.Where(DATA => DATA.idAccount == oDATA.idRow && DATA.RowStatusValue == (short)Model.Base.enRowStatus.Active).Count() != 0)
            {
               oReturn.MSG.Add(new Message() { Warning = Resources.Accounts.MSG_WARNING_DATA_BEING_RELATED });
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
            var oData = ((Model.Account)oParameters.DATA[TAG_ENTITY]);

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
            using (var oService = new Service.Account(oParameters.Login))
            {

               // QUERY
               var oList = oService.GetDataList(oParameters.Login, oParameters.GetTextData(TAG_SEARCH));

               // QUERY: AUTO COMPLETE
               var oResultQuery =
                  from DATA in oList
                  where DATA.Status == Model.Account.enStatus.Enabled
                  select new AutoCompleteData
                  {
                     Value = DATA.idAccount.ToString(),
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
