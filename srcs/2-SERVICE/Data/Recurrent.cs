using System;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;
using FriendCash.Model.Tools;

namespace FriendCash.Service
{
   public class Recurrent : Base
   {

      #region New
      internal Recurrent(Model.Login oLogin) : base(oLogin) { }
      #endregion

      #region Constants
      public const string TAG_ENTITY = "Recurrent";
      public const string TAG_ENTITY_LIST = TAG_ENTITY + "s";
      public const string TAG_ENTITY_KEY = "id" + TAG_ENTITY;
      #endregion

      #region GetQuery
      internal IQueryable<Model.Recurrent> GetQuery(Parameters oParameters)
      {
         IQueryable<Model.Recurrent> oQuery = null;

         // INITIAL
         oQuery = from DATA in this.Context.Recurrents
                  where DATA.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                  select DATA;
         this.ApplyLogin(ref oQuery, oParameters.Login);

         // ID
         long idRow = oParameters.GetNumericData(TAG_ID);
         if (idRow != 0) { oQuery = oQuery.Where(DATA => DATA.idRow == idRow); }

         // KEY
         long idKey = oParameters.GetNumericData(TAG_ENTITY_KEY);
         if (idKey != 0) { oQuery = oQuery.Where(DATA => DATA.idRecurrent == idKey); }

         // DOCUMENT
         long idDocument = oParameters.GetNumericData("idDocument");
         if (idDocument != 0) { oQuery = oQuery.Where(DATA => DATA.idDocument == idDocument); }

         // ORDER BY
         oQuery = oQuery.OrderBy(DATA => DATA.idRecurrent);

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
            { this.ApplyPagination<Service.Recurrent, Model.Recurrent>(ref oQuery, oParameters, ref oReturn); }

            // EXECUTE
            oReturn.DATA.Add(TAG_ENTITY_LIST, oQuery.ToList<Model.Recurrent>());
            oReturn.DATA.Add(TAG_SEARCH, oParameters.GetTextData(TAG_SEARCH));

            // OK
            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }
      #endregion

      #region GetDataSingle

      internal Model.Recurrent GetDataSingle_ByKey(Model.Login oLogin, long idRecurrent)
      { return this.GetDataSingle(oLogin, 0, idRecurrent, 0); }

      internal Model.Recurrent GetDataSingle_ByID(Model.Login oLogin, long idRow)
      { return this.GetDataSingle(oLogin, idRow, 0, 0); }

      internal Model.Recurrent GetDataSingle_ByDocument(Model.Login oLogin, long idDocument)
      { return this.GetDataSingle(oLogin, 0, 0, idDocument); }

      private Model.Recurrent GetDataSingle(Model.Login oLogin, long idRow, long idKey, long idDocument)
      {
         Model.Recurrent oReturn = null;

         try
         {

            // PARAMETERS
            var oParameters = new Parameters();
            oParameters.DATA.Add(TAG_ID, idRow);
            oParameters.DATA.Add(TAG_ENTITY_KEY, idKey);
            oParameters.DATA.Add("idDocument", idDocument);
            if (oLogin != null) { oParameters.Login = oLogin; }

            // EXECUTE
            Return oExecReturn = this.GetData(oParameters, false);

            // RESULT
            if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oExecReturn.DATA[TAG_ENTITY_LIST] != null)
            {
               var oList = ((List<Model.Recurrent>)oExecReturn.DATA[TAG_ENTITY_LIST]);
               oReturn = oList.SingleOrDefault();
            }

         }
         catch { throw; }

         return oReturn;
      }

      #endregion

      #region GetDataList
      internal List<Model.Recurrent> GetDataList(Model.Login oLogin, string Search)
      {
         List<Model.Recurrent> oReturn = null;

         // PARAMETERS
         var oParameters = new Parameters();
         oParameters.DATA.Add(TAG_SEARCH, Search);
         oParameters.Login = oLogin;

         // EXECUTE
         var oExecReturn = this.GetData(oParameters, false);

         // RESULT
         if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oExecReturn.DATA[TAG_ENTITY_LIST] != null)
         {
            oReturn = ((List<Model.Recurrent>)oExecReturn.DATA[TAG_ENTITY_LIST]);
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
            using (var oService = new Service.Recurrent(oParameters.Login))
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

            // PARAMETERS
            var iType = ((Model.Document.enType)oParameters.GetNumericData("Type"));

            // EMPTY MODEL
            oReturn.DATA.Add(TAG_ENTITY, new Model.Recurrent() { Type = iType });

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
            using (var oService = new Service.Recurrent(oParameters.Login))
            {

               // EXECUTE
               oReturn = oService.GetData(oParameters, false);

               // RESULT
               if (oReturn.OK == true && oReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oReturn.DATA[TAG_ENTITY_LIST] != null)
               {
                  var oList = ((List<Model.Recurrent>)oReturn.DATA[TAG_ENTITY_LIST]);
                  if (oList.Count == 0) { oReturn.MSG.Add(new Message() { Warning = Resources.Recurrent.MSG_WARNING_NOT_FOUND }); oReturn.OK = false; }
                  if (oList.Count >= 2) { oReturn.MSG.Add(new Message() { Warning = Resources.Recurrent.MSG_WARNING_DUPLICITY }); oReturn.OK = false; }
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
            using (var oService = new Service.Recurrent(oParameters.Login))
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
         var oData = ((Model.Recurrent)oParameters.DATA[TAG_ENTITY]);

         // DOCUMENT
         if (oData.idDocument == 0)
         { oReturn.MSG.Add(new Message() { Warning = Resources.Recurrent.MSG_REQUIRED_DOCUMENT }); return false; }

         // VALUE
         if (oData.Value == 0)
         { oReturn.MSG.Add(new Message() { Warning = Resources.Recurrent.MSG_REQUIRED_VALUE }); return false; }

         return true;
      }

      private bool SaveEdit_Apply(Return oReturn, Parameters oParameters)
      {
         bool bReturn = false;

         try
         {

            // DATA
            var oData = ((Model.Recurrent)oParameters.DATA[TAG_ENTITY]);

            // APPLY
            var oAfterSave = new Action<Model.Recurrent>(DATA => { if (DATA.idRecurrent == 0) { DATA.idRecurrent = DATA.idRow; } });
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
            using (var oService = new Service.Recurrent(oParameters.Login))
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
               oReturn.MSG.Add(new Message() { Warning = Resources.Recurrent.MSG_WARNING_NOT_FOUND });
               return bReturn;
            }

            // CHECK EXISTENCE
            var oDATA = ((Model.Recurrent)oParameters.DATA[TAG_ENTITY]);
            if (this.Context.Recurrents.Where(DATA => DATA.idRow == oDATA.idRow).Count() == 0)
            {
               oReturn.MSG.Add(new Message() { Warning = Resources.Recurrent.MSG_WARNING_NOT_FOUND });
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
            var oData = ((Model.Recurrent)oParameters.DATA[TAG_ENTITY]);

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

      #endregion

   }
}
