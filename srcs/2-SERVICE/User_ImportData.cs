using System;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;

namespace FriendCash.Service
{
   internal class ImportData
   {

      #region Common

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

      #region Validate
      internal bool Validate(ref Return oReturn, System.IO.Stream oStream)
      {

         // CHECK FILE
         if (oStream == null) { oReturn.MSG.Add("Invalid File"); return false; }

         // PARAMETERS
         char oSep = ((char)";".ToCharArray().GetValue(0));
         string sValidHeader = "Document_Description;Document_Type;Supplier_Code;Supplier_Description;Planning_Description;History_DueDate;History_Value;History_Settled;History_PayDate;Account_Code;Account_Description;Transfer";
         string sHeader = string.Empty;

         // READS HEADER
         try
         {
            System.IO.StreamReader oReader = new System.IO.StreamReader(oStream);
            sHeader = oReader.ReadLine();
         }
         catch (Exception ex) { oReturn.MSG.Add("Invalid File Header [" + ex.Message + "]"); return false; }
         if (string.IsNullOrEmpty(sHeader)) { oReturn.MSG.Add("Invalid File Header"); return false; }

         // CHECK HEADER
         string[] aHeaders = sHeader.Split(oSep);
         string[] aValidHeaders = sValidHeader.Split(oSep);
         if (aHeaders == null || aHeaders.Length != aValidHeaders.Length) { oReturn.MSG.Add("Invalid File Header"); return false; }
         for (int iIndex = 0; iIndex < (aValidHeaders.Length - 1); iIndex++)
         {
            if (aHeaders[iIndex].ToString().ToUpper() != aValidHeaders[iIndex].ToString().ToUpper())
            {
               oReturn.MSG.Add("Invalid File Header"); return false;
            }
         }

         return true;
      }
      #endregion

      #region Execute

      internal bool Execute(ref Return oReturn, System.IO.Stream oStream, Model.Login oLogin, List<string> oMSG)
      {
         System.IO.StreamReader oReader = null;

         try
         {

            // CHECK FILE
            if (oStream == null) { oReturn.MSG.Add("Invalid File"); return false; }

            // PARAMETERS
            char oSep = ((char)";".ToCharArray().GetValue(0));

            // SERVICES
            Supplier oSupplier = new Supplier();
            Planning oPlanning = new Planning();
            Document oDocument = new Document();

            // REMOVE CURRENT DATA
            if (this.Execute_Remove(ref oReturn, oMSG, oLogin, oSupplier, oPlanning, oDocument) == false)
             { return false; }

            // READS DATA
            oReader = new System.IO.StreamReader(oStream);
            string sTemp = oReader.ReadLine();
            do
            {
               string sRow = oReader.ReadLine();
               string[] aRow = sRow.Split(oSep);

               if (aRow != null && aRow.Length == ((int)enImportColumns.TOTAL))
               {
                  string Document_Description = this.Execute_GetRowValue(aRow, enImportColumns.Document_Description, "");
                  string Document_Type = this.Execute_GetRowValue(aRow, enImportColumns.Document_Type, "0");
                  string Supplier_Code = this.Execute_GetRowValue(aRow, enImportColumns.Supplier_Code, "");
                  string Supplier_Description = this.Execute_GetRowValue(aRow, enImportColumns.Supplier_Description, "");
                  string Planning_Description = this.Execute_GetRowValue(aRow, enImportColumns.Planning_Description, "");
                  string History_DueDate = this.Execute_GetRowValue(aRow, enImportColumns.History_DueDate, DateTime.MinValue.ToString("yyyy-MM-dd"));
                  string History_Value = this.Execute_GetRowValue(aRow, enImportColumns.History_Value, "0");
                  string History_Settled = this.Execute_GetRowValue(aRow, enImportColumns.History_Settled, "0");
                  string History_PayDate = this.Execute_GetRowValue(aRow, enImportColumns.History_PayDate, DateTime.MinValue.ToString("yyyy-MM-dd"));
                  string Account_Code = this.Execute_GetRowValue(aRow, enImportColumns.Account_Code, "");
                  string Account_Description = this.Execute_GetRowValue(aRow, enImportColumns.Account_Description, "");
                  string Transfer = this.Execute_GetRowValue(aRow, enImportColumns.Transfer, "0");

                  Model.Document.enType iType = this.GetType(Document_Type);
                  long idSupplier = this.GetSupplier(oLogin, oMSG, oSupplier, Supplier_Code, Supplier_Description);
                  long idPlanning = this.GetPlanning(oLogin, oMSG, oPlanning, Planning_Description, iType);
                  long idDocument = this.GetDocument(oLogin, oMSG, oDocument, Document_Description, Transfer, iType, idSupplier, idPlanning);

               }

            } while (oReader.EndOfStream == false);

         }
         catch (Exception) { throw; }

         return true;
      }

      private string Execute_GetRowValue(string[] aRow, enImportColumns iColumn, string oDefault)
      {
         string oReturn = oDefault;

         try
         {

            int iColumnValue = ((int)iColumn);
            if (aRow.Length >= iColumnValue)
            {
               oReturn = aRow.GetValue(iColumnValue).ToString().Trim();
            }

         }
         catch (Exception) { }

         return oReturn;
      }

      private bool Execute_Remove(ref Return oReturn, List<string> oMSG, Model.Login oLogin, Supplier oSupplier, Planning oPlanning, Document oDocument)
      {
         bool bReturn = false;

         try
         {

            // SUPPLIER
            try
            {
               List<Model.Supplier> oSupplierModel = oSupplier.GetAll(oLogin);
               if (oSupplierModel != null && oSupplierModel.Count != 0)
               {
                  oSupplierModel.ForEach(oItem =>
                     {
                        oItem.RowStatus = Model.Base.enRowStatus.Removed;
                        oItem.RemovedBy = oLogin.idRow;
                        oItem.RemovedIn = DateTime.Now;
                     });
                  oSupplier.Context.SaveChanges(oMSG);
               }
            }
            catch (Exception exInner) { throw new Exception("Error Removing Supplier Records [" + exInner.Message + "]"); }

            // PLANNING
            try
            {
               List<Model.Planning> oPlanningModel = oPlanning.GetAll(oLogin);
               if (oPlanningModel != null && oPlanningModel.Count != 0)
               {
                  oPlanningModel.ForEach(oItem =>
                  {
                     oItem.RowStatus = Model.Base.enRowStatus.Removed;
                     oItem.RemovedBy = oLogin.idRow;
                     oItem.RemovedIn = DateTime.Now;
                  });
                  oPlanning.Context.SaveChanges(oMSG);
               }
            }
            catch (Exception exInner) { throw new Exception("Error Removing Planning Records [" + exInner.Message + "]"); }

            // PLANNING
            try
            {
               List<Model.Document> oDocumentModel = oDocument.GetAll(oLogin);
               if (oDocumentModel != null && oDocumentModel.Count != 0)
               {
                  oDocumentModel.ForEach(oItem =>
                  {
                     oItem.RowStatus = Model.Base.enRowStatus.Removed;
                     oItem.RemovedBy = oLogin.idRow;
                     oItem.RemovedIn = DateTime.Now;
                  });
                  oDocument.Context.SaveChanges(oMSG);
               }
            }
            catch (Exception exInner) { throw new Exception("Error Removing Document Records [" + exInner.Message + "]"); }

            // OK
            bReturn = true;

          }
         catch (Exception ex) { oMSG.Add(ex.Message); }

         return bReturn;
       }

      #endregion

      #region GetData

      private Model.Document.enType GetType(string Document_Type)
      {
         Model.Document.enType iType = Model.Document.enType.None;

         try
         {

            if (Document_Type == "Expense")
             { iType = Model.Document.enType.Expense; }
            else if (Document_Type == "Income")
             { iType = Model.Document.enType.Income; }


         }
         catch (Exception) { }

         return iType;
      }

      private long GetSupplier(Model.Login oLogin, List<string> oMSG, Supplier oService, string Supplier_Code, string Supplier_Description)
      {
         long idSupplier = 0;

         try
         {

            Model.Supplier oModel = oService.GetData_ByCode(oLogin, Supplier_Code);
            if (oModel == null || oModel.idSupplier == 0)
            {
               oModel = new Model.Supplier();
               oModel.Code = Supplier_Code;
               oModel.Description = Supplier_Description;
               idSupplier = oService.Add(oLogin, oMSG, oModel);
             }

            if (oModel != null)
             { idSupplier = oModel.idSupplier; }

          }
         catch (Exception) { }

         return idSupplier;
       }

      private long GetPlanning(Model.Login oLogin, List<string> oMSG, Planning oService, string Planning_Description, Model.Document.enType iType)
      {
         long idPlanning = 0;

         try
         {

            char oSep = ((char)"/".ToCharArray().GetValue(0));
            string[] aPlanning = Planning_Description.Split(oSep);
            long idParentRow = 0;

            foreach (string sPlanning in aPlanning)
            {
               if (!string.IsNullOrEmpty(sPlanning))
               {
                  Model.Planning oModel = oService.GetData_ByCode(sPlanning, idParentRow);
                  if (oModel == null || oModel.idPlanning == 0)
                  {
                     oModel = new Model.Planning();
                     oModel.Description = Planning_Description;
                     oModel.Type = iType;
                     if (idParentRow != 0) { oModel.idParentRow = idParentRow; }
                     idPlanning = oService.Add(oLogin, oMSG, oModel);
                     idParentRow = idPlanning;
                  }

                  if (oModel != null)
                  { idPlanning = oModel.idPlanning; idParentRow = idPlanning; }
               }
             }

         }
         catch (Exception) { }

         return idPlanning;
      }

      private long GetDocument(Model.Login oLogin, List<string> oMSG, Document oService, string Document_Description, string Transfer, Model.Document.enType iType, long idSupplier, long idPlanning)
      {
         long idDocument = 0;

         try
         {

            if (string.IsNullOrEmpty(Document_Description) && !string.IsNullOrEmpty(Transfer))
             { Document_Description = "Transfer"; }

            Model.Document oModel = oService.GetData_ByCode(oLogin, Document_Description);
            if (oModel == null || oModel.idDocument == 0)
            {
               oModel = new Model.Document();
               oModel.Description = Document_Description;
               oModel.Type = iType;
               oModel.idSupplier = idSupplier;
               oModel.idPlanning = idPlanning;
               idDocument = oService.Add(oLogin, oMSG, oModel);
             }

            if (oModel != null) { idDocument = oModel.idDocument; }

         }
         catch (Exception) { }

         return idDocument;
      }

      #endregion

   }
}
