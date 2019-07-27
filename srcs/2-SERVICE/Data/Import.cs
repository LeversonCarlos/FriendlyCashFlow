using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FriendCash.Model.Tools;
using System.Threading.Tasks;

namespace FriendCash.Service
{

   #region Import
   public class Import : Base
   {

      #region New
      internal Import(Model.Login oLogin) : base(oLogin) { }
      #endregion

      #region Constants

      public const string TAG_ENTITY = "Import";
      public const string TAG_ENTITY_LIST = TAG_ENTITY + "s";
      public const string TAG_ENTITY_KEY = "id" + TAG_ENTITY;

      private const string TAG_STATUS = "Status";
      private const string TAG_STREAM_FILE = "STREAM_FILE";
      private const string TAG_TRANSFER_KEYS = "TransferKeys";
      private char TAG_SEP = ((char)";".ToCharArray().GetValue(0));
      private const string TAG_PROGRESS_TOTAL = "ProgressTotal";
      private const string TAG_PROGRESS_EXECUTED = "ProgressExecuted";
      private const string TAG_PROGRESS_PERCENT = "ProgressPercent";

      private const string TAG_VALID_HEADER = "Document_Description;Document_Type;Supplier_Code;Supplier_Description;Planning_Description;History_DueDate;History_Value;History_Settled;History_PayDate;Account_Code;Account_Description;Transfer";

      private enum enImportColumns : int
      {
         Document_Description = 0,
         Document_Type = 1,
         Supplier_Code = 2,
         Supplier_Description = 3,
         Planning_Description = 4,
         History_DueDate = 5,
         History_Value = 6,
         History_Settled = 7,
         History_PayDate = 8,
         Account_Code = 9,
         Account_Description = 10,
         Transfer = 11,
         TOTAL = 12
      }

      #endregion

      #region GetQuery
      private IQueryable<Model.Import> GetQuery(Parameters oParameters)
      {
         IQueryable<Model.Import> oQuery = null;

         // QUERY
         oQuery = from DATA in this.Context.Imports
                  where DATA.RowStatusValue == ((short)Model.Base.enRowStatus.Active)
                  select DATA;
         //this.ApplyLogin(ref oQuery, oParameters.Login);

         // ID
         long idRow = oParameters.GetNumericData(TAG_ID);
         if (idRow != 0)
         { oQuery = oQuery.Where(DATA => DATA.idRow == idRow); }

         // KEY
         long Key = oParameters.GetNumericData(TAG_ENTITY_KEY);
         if (Key != 0)
         { oQuery = oQuery.Where(DATA => DATA.idImport == Key); }

         // STATUS
         long iStatus = oParameters.GetNumericData(TAG_STATUS);
         if (iStatus != 0)
         { oQuery = oQuery.Where(DATA => DATA.StatusValue == ((short)iStatus)); }

         // ORDER BY
         oQuery = oQuery.OrderByDescending(DATA => DATA.idImport);
         oQuery = oQuery.Take(5);

         return oQuery;
      }
      #endregion 

      #region GetData

      #region GetData
      private Return GetData(Parameters oParameters)
      {
         Return oReturn = new Return();

         try
         {

            // QUERY
            var oQuery = this.GetQuery(oParameters); 

            // EXECUTE
            var oDATA = oQuery.ToList<Model.Import>();
            oReturn.DATA.Add(TAG_ENTITY_LIST, oDATA);

            // OK
            oReturn.OK = true;

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }
      #endregion

      #region GetDataGrouped
      public static Return GetDataGrouped(Parameters oParameters)
      {
         var oReturn = new Return();

         try
         {

            // INSTANCE
            using (var oService = new Service.Import(oParameters.Login))
            {

               // IMPORTS
               var oImports = oService.GetQuery(oParameters);

               // JOIN 
               var oJoined = from Imports in oImports
                             join Status in oService.Context.ImportsStatus on new { Imports.idImport, Imports.StatusValue } equals new { Status.idImport, Status.StatusValue }
                             orderby Imports.idImport descending
                             select new Model.ViewImport
                             {
                                idImport = Imports.idImport,
                                StartDate = Imports.StartDate,
                                FinishDate = Imports.FinishDate,
                                StatusValue = Imports.StatusValue,
                                Message = Imports.Message,
                                StepDate = Status.StepDate,
                                ProgressTotal = Status.ProgressTotal,
                                ProgressCompleted = Status.ProgressCompleted,
                                ImportedSucess = (from Datas in oService.Context.ImportDatas
                                                  where Datas.idImport == Imports.idImport &&
                                                        Datas.StatusValue == ((short)Model.ImportData.enStatus.Success)
                                                  select Datas.idRow).Count(),
                                ImportedError = (from Datas in oService.Context.ImportDatas
                                                 where Datas.idImport == Imports.idImport &&
                                                       Datas.StatusValue == ((short)Model.ImportData.enStatus.Error)
                                                 select Datas.idRow).Count()
                             };

               // DATA
               oJoined = oJoined.Take(5);
               var oDATA = oJoined.ToList();
               oReturn.DATA.Add(TAG_ENTITY_LIST, oDATA);

               // EXECUTING IMPORT
               bool bExecuting = false;
               if (oDATA.Where(x => x.StatusValue < ((short)Model.Import.enStatus.Finished)).Count() != 0) { bExecuting = true; }
               oReturn.DATA.Add("ImportExecuting", bExecuting);

               // OK
               oReturn.OK = true;

            }

         }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
      }
      #endregion

      #region GetDataStatus
      private Model.ImportStatus GetDataStatus(Parameters oParameters, long idImport, Model.Import.enStatus iStatus)
      {
         Model.ImportStatus oStatus = null;

         try
         {

            // IMPORTS
            var oImports = this.GetQuery(oParameters);

            // JOIN 
            var oJoined = from Imports in oImports
                          join Status in this.Context.ImportsStatus on Imports.idImport equals Status.idImport
                          where
                             Imports.idImport == idImport &&
                             Status.RowStatusValue == ((short)Model.Base.enRowStatus.Active) &&
                             Status.StatusValue == ((short)iStatus)
                          select Status;

            // DATA
            var oDATA = oJoined.ToList();
            oStatus = oDATA.SingleOrDefault();

         }
         catch { }

         return oStatus;
      }
      #endregion
      
      #region GetData_By

      /*
      internal Model.Import GetData_ByStatus(Model.Login oLogin, Model.Import.enStatus iStatus)
      { return this.GetData_By(oLogin, 0, 0, iStatus); }

      internal Model.Import GetData_ByStatus(Model.Login oLogin)
      { return this.GetData_By(oLogin, 0, 0, Model.Import.enStatus.Waiting); }
      */

      internal Model.Import GetData_ByKey(Model.Login oLogin, long idImport)
      { return this.GetData_By(null, 0, idImport, Model.Import.enStatus.Waiting); }

      internal Model.Import GetData_ByID(long idRow)
      { return this.GetData_By(null, idRow, 0, Model.Import.enStatus.Waiting); }

      private Model.Import GetData_By(Model.Login oLogin, long idRow, long idImport, Model.Import.enStatus iStatus)
      {
         Model.Import oReturn = null;

         try
         {

            // PARAMETERS
            var oParameters = new Parameters();
            oParameters.Login = oLogin;
            oParameters.DATA.Add(TAG_ID, idRow);
            oParameters.DATA.Add(TAG_ENTITY_KEY, idImport);
            oParameters.DATA.Add(TAG_STATUS, ((long)iStatus));

            // EXECUTE
            var oExecReturn = this.GetData(oParameters);

            // RESULT
            if (oExecReturn.OK == true && oExecReturn.DATA.ContainsKey(TAG_ENTITY_LIST) && oExecReturn.DATA[TAG_ENTITY_LIST] != null)
            {
               List<Model.Import> oList = ((List<Model.Import>)oExecReturn.DATA[TAG_ENTITY_LIST]);
               oReturn = oList.SingleOrDefault();
             }

         }
         catch (Exception ex) { throw ex; }

         return oReturn;
      }

      #endregion

      #endregion 

      #region Start

      public static Return Start(Parameters oParameters)
      {
         var oReturn = new Return();

         try
         {

            // INSTANCE
            using (var oService = new Service.Import(oParameters.Login))
            {

               // CREATES IMPORT HEADER
               long idImport = -1;
               oParameters.DATA.Add(TAG_ENTITY_KEY, idImport);
               if (oService.Execute_Update(oParameters, ref idImport) == false) { return oReturn; }
               oParameters.DATA[TAG_ENTITY_KEY] = idImport;

               // STREAM
               string sStreamFilePath = oParameters.GetTextData(TAG_STREAM_FILE);
               string sMessage = string.Empty;
               if (!oService.Start_Validate(sMessage, sStreamFilePath))
               {
                  oService.Execute_Update(oParameters, Model.Import.enStatus.Canceled, sMessage);
                  oReturn.MSG.Add(new Message() { Warning = sMessage });
                  return oReturn;
               }

               // THREAD
               oService.Start_Thread(oParameters);

               // OK
               oReturn.DATA.Add(TAG_ENTITY_KEY, idImport);
               oReturn.OK = true;

            }

          }
         catch (Exception ex) { oReturn.MSG.Add(new Message() { Exception = ex.Message }); }

         return oReturn;
       }

      private bool Start_Validate(string sMessage, string sStreamFilePath)
      {

         // CHECK FILE
         if (string.IsNullOrEmpty(sStreamFilePath)) { sMessage = "Invalid File"; return false; }
         if (!System.IO.File.Exists(sStreamFilePath)) { sMessage = "Invalid File"; return false; }

         // READS STREAM HEADER
         string sHeader = string.Empty;
         try
         {
            using (System.IO.StreamReader oReader = new System.IO.StreamReader(sStreamFilePath))
            {
               if (oReader == null) { sMessage = "Invalid File"; return false; }
               sHeader = oReader.ReadLine();
             }
         }
         catch (Exception ex) { sMessage = "Invalid File Header [" + ex.Message + "]"; return false; }
         if (string.IsNullOrEmpty(sHeader)) { sMessage = "Invalid File Header"; return false; }

         // CHECK HEADER
         string[] aHeaders = sHeader.Split(TAG_SEP);
         string[] aValidHeaders = TAG_VALID_HEADER.Split(TAG_SEP);
         if (aHeaders == null || aHeaders.Length != aValidHeaders.Length) { sMessage = "Invalid File Header"; return false; }
         for (int iIndex = 0; iIndex < (aValidHeaders.Length - 1); iIndex++)
         {
            if (aHeaders[iIndex].ToString().ToUpper() != aValidHeaders[iIndex].ToString().ToUpper())
            {
               sMessage = "Invalid File Header"; return false;
            }
         }

         return true;
      }

      private void Start_Thread(Parameters oParameters)
      {
         var oThread = new System.Threading.Thread(x => Import.Execute_DoWork(oParameters));
         oThread.Start();
         //await Task.Run(() => Execute_DoWork(oParameters));
      }

      #endregion

      #region Execute

      #region Execute_DoWork
      private static void Execute_DoWork(Parameters oParameters)
      {
         string sMessage = string.Empty;

         using (var oService = new Service.Import(oParameters.Login))
         {
            try
            {

               // CLEAR CURRENT DATA
               if (!oService.Execute_Clear(oParameters, ref sMessage))
               { oService.Execute_Update(oParameters, Model.Import.enStatus.Canceled, sMessage); return; }

               // READ AND STORE STREAM
               if (!oService.Execute_Stream(oParameters, ref sMessage))
               { oService.Execute_Update(oParameters, Model.Import.enStatus.Canceled, sMessage); return; }

               // FINISH IMPORT
               oService.Execute_Update(oParameters, Model.Import.enStatus.Finished, string.Empty);

            }
            catch (Exception ex) { oService.Execute_Update(oParameters, Model.Import.enStatus.Canceled, ex.Message); }
         }
       }
      #endregion

      #region Execute_Stream
      private bool Execute_Stream(Parameters oParameters, ref string sMessage)
      {
         bool bReturn = false;

         try
         {

            // STATUS
            this.Execute_Update(oParameters, Model.Import.enStatus.Streaming, string.Empty);
            
            // PARAMETERS
            long idImport = ((long)oParameters.DATA[TAG_ENTITY_KEY]);
            string sStreamFilePath = oParameters.GetTextData(TAG_STREAM_FILE);

            // PROGRESS
            Model.ImportStatus oStatus = this.GetDataStatus(oParameters, idImport, Model.Import.enStatus.Streaming);

            // READS DATA
            using (System.IO.StreamReader oReader = new System.IO.StreamReader(sStreamFilePath))
            {
               oStatus.ProgressTotal = oReader.BaseStream.Length;
               string sData = oReader.ReadLine(); // SKIP HEADER
               do 
               {

                  try
                  {

                     // PROGRESS
                     this.Execute_Update_Status(oStatus.idRow, oReader.BaseStream.Position, oStatus.ProgressTotal);

                     // DATA
                     sData = oReader.ReadLine();
                     if (!string.IsNullOrEmpty(sData))
                     {
                        var sQUERY = "INSERT INTO v4_ImportData(idImport, Data, Status, RowStatus, CreatedBy, CreatedIn) " +
                                     "VALUES (@idImport, @Data, 0, @RowStatus, @CreatedBy, @CreatedIn) ";
                        var oPARAM = new System.Data.SqlClient.SqlParameter[] { 
                        new System.Data.SqlClient.SqlParameter("idImport", idImport), 
                        new System.Data.SqlClient.SqlParameter("Data", sData), 
                        new System.Data.SqlClient.SqlParameter("RowStatus", (short)Model.Base.enRowStatus.Active), 
                        new System.Data.SqlClient.SqlParameter("CreatedBy", oParameters.Login.idRow), 
                        new System.Data.SqlClient.SqlParameter("CreatedIn", DateTime.Now )
                     };
                        var iAFFECTED = this.Context.Database.ExecuteSqlCommand(sQUERY, oPARAM);
                     }
                  }
                  catch (Exception exInner) {  System.Diagnostics.Debug.WriteLine(sData + Environment.NewLine + exInner.ToString()); }

                } while (oReader.EndOfStream == false);
             }

            // DELETE STREAM FILE
            try { System.IO.File.Delete(sStreamFilePath); }
            catch { }

            // UPDATE DOCUMENT SETTLED
            try
            {
               var sQUERY = "" +
                  "UPDATE v4_Documents " +
                  "SET Settled = 0 " + 
                  "WHERE idRow in " + 
                  "(" +
                     "SELECT idDocument " + 
                     "FROM v4_History " + 
                     "WHERE " + 
                        "Settled = 0 And " + 
                        "RowStatus = 1 And " + 
                        "CreatedBy = @iLogin " + 
                     "GROUP BY idDocument " + 
                   ") ";
               var oPARAM = new System.Data.SqlClient.SqlParameter[] 
                  { new System.Data.SqlClient.SqlParameter("iLogin", oParameters.Login.idRow) };
               var iAFFECTED = this.Context.Database.ExecuteSqlCommand(sQUERY, oPARAM);
            }
            catch { }

            // PROGRESS
            this.Execute_Update_Status(oStatus.idRow, oStatus.ProgressTotal, oStatus.ProgressTotal);

            // OK
            bReturn = true;

          }
         catch (Exception ex) { sMessage = ex.Message; }

         return bReturn;
       }
      #endregion

      #region Execute_Clear
      private bool Execute_Clear(Parameters oParameters, ref string sMessage)
      {
         bool bReturn = false;

         try
         {

            // STATUS
            this.Execute_Update(oParameters, Model.Import.enStatus.Clearing, string.Empty);
            long iProgressTotal = 6; long iProgressCompleted = 0;

            iProgressCompleted = 1;
            this.Execute_Update_Status(oParameters, Model.Import.enStatus.Clearing, iProgressTotal, iProgressCompleted);

            // EXECUTE
            var sQUERY = "EXEC dbo.v4_ImportClear @iLogin ";
            var oPARAM = new System.Data.SqlClient.SqlParameter[] 
                         { 
                           new System.Data.SqlClient.SqlParameter("iLogin", oParameters.Login.idRow)
                          };
            var iAFFECTED = this.Context.Database.ExecuteSqlCommand(sQUERY, oPARAM);

            // STATUS
            iProgressCompleted = 6; 
            this.Execute_Update_Status(oParameters, Model.Import.enStatus.Clearing, iProgressTotal, iProgressCompleted);

            // OK
            bReturn = true;

         }
         catch (Exception ex) { sMessage = ex.Message; }

         return bReturn;
      }
      #endregion

      #region Execute_Update

      private bool Execute_Update(Parameters oParameters, ref long idImport)
      {
         bool bReturn = false;
         try
         {
            var oValue = new Model.Import();
            bReturn = this.Execute_Update(oParameters, oValue, Model.Import.enStatus.Starting, string.Empty);
            if (bReturn) { idImport = oValue.idImport; }
          }
         catch { throw; }
         return bReturn;
       }

      private bool Execute_Update(Parameters oParameters, Model.Import.enStatus iStatus, string sMessage)
      {
         bool bReturn = false;
         try
         {
            long idImport = ((long)oParameters.DATA[TAG_ENTITY_KEY]);
            Model.Import oValue = this.GetData_ByKey(oParameters.Login, idImport);
            if (oValue != null)
            {
               bReturn = this.Execute_Update(oParameters, oValue, iStatus, sMessage);
            }
          }
         catch { }
         return bReturn;
       }

      private bool Execute_Update(Parameters oParameters, Model.Import oImport, Model.Import.enStatus iStatus, string sMessage)
      {
         bool bReturn = false;

         try
         {

            // IMPORT: DATA
            var oMSG = new List<Message>();
            oImport.Message = sMessage;
            oImport.Status = iStatus;
            if (iStatus == Model.Import.enStatus.Finished) { oImport.FinishDate = DateTime.Now; }

            // IMPORT: APPLY
            var oImportAfterSave = new Action<Model.Import>(DATA => { if (DATA.idImport == 0) { DATA.idImport = DATA.idRow; } });
            if (this.ApplySave(oImport, oParameters.Login, oMSG, oImportAfterSave, false) == false) { return bReturn; }

            // STATUS: DATA
            var oStatus = new Model.ImportStatus();
            oStatus.idImport = oImport.idImport;
            oStatus.Status = oImport.Status;
            oStatus.StepDate = DateTime.Now;

            // STATUS: APPLY
            if (this.ApplySave(oStatus, oParameters.Login, oMSG, null, false) == false) { return bReturn; }

            // OK
            bReturn = true; 

         }
         catch { }

         return bReturn;
       }

      private void Execute_Update_Status(Parameters oParameters, Model.Import.enStatus iStatus, long iProgressTotal, long iProgressCompleted)
      {
         try
         {

            // CHECK PERCENTAGE
            double dPerc = Convert.ToDouble(iProgressCompleted) / Convert.ToDouble(iProgressTotal) * 100;
            if (Convert.ToInt32(dPerc) != ((Int32)dPerc)) { return; }

            // APPLY CHANGES
            long idImport = ((long)oParameters.DATA[TAG_ENTITY_KEY]);
            Model.ImportStatus oStatus = this.GetDataStatus(oParameters, idImport, iStatus);
            oStatus.ProgressTotal = iProgressTotal;
            oStatus.ProgressCompleted = iProgressCompleted;

            // UPDATE
            this.Context.ImportsStatus.Attach(oStatus);
            this.Context.Entry<Model.ImportStatus>(oStatus).State = System.Data.Entity.EntityState.Modified;
            this.Context.SaveChanges(null);

         }
         catch { }
      }

      private void Execute_Update_Status(long idRow, long ProgressCompleted, long ProgressTotal)
      {
         try
         {
            var sQUERY = "UPDATE v4_ImportStatus SET ProgressCompleted=@ProgressCompleted, ProgressTotal=@ProgressTotal WHERE idRow=@idRow";
            var oPARAM = new System.Data.SqlClient.SqlParameter[] 
                         {
                           new System.Data.SqlClient.SqlParameter("ProgressCompleted", ProgressCompleted), 
                           new System.Data.SqlClient.SqlParameter("ProgressTotal", ProgressTotal), 
                           new System.Data.SqlClient.SqlParameter("idRow", idRow)
                          };
            var iAFFECTED = this.Context.Database.ExecuteSqlCommand(sQUERY, oPARAM);
         }
         catch { }
      }

      #endregion

      #endregion

    }
   #endregion 

}
