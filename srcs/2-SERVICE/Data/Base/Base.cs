using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FriendCash.Model.Tools;

namespace FriendCash.Service
{
   public abstract class Base: IDisposable
   {

      #region New
      //public Base() { }
      public Base(Model.Context oContext) { this.Context = oContext; }
      public Base(Model.Login oLogin) 
      {
         if (System.Configuration.ConfigurationManager.AppSettings["SqlCompact"] == "1")
         { this.Context = new Model.Context(oLogin); }
         else { this.Context = new Model.Context(); }
       }
      #endregion

      #region Constants
      public const string TAG_ID = "idRow";
      public const string TAG_SEARCH = "Search"; 
      #endregion

      #region Properties

      #region Context
      private Model.Context oContext = null;
      public Model.Context Context 
      {
         get
         {
            //if (this.oContext == null) { this.oContext = new Model.Context(); }
            return this.oContext;
          }
         internal set { this.oContext = value; }
       }
      #endregion

      #region AuditTrailActivated
      private bool AuditTrailActivated
      { get { return (System.Configuration.ConfigurationManager.AppSettings["AuditTrailActivated"] == "1"); } }
      #endregion

      #endregion

      #region GetMaxID
      protected long GetMaxID<tModel>() where tModel : Model.Base
      {
         long iReturn = 0;

         try
         {

            var oExec = (from DATA in this.Context.Set<tModel>() select DATA.idRow);
            if (oExec != null && oExec.Count<long>() > 0)
            { iReturn = oExec.Max<long>(); }
            iReturn += 1;

         }
         catch { throw; }

         return iReturn;
      }
      #endregion

      #region ApplyLogin
      protected void ApplyLogin<M>(ref IQueryable<M> oQuery, Model.Login oLogin) where M : Model.Base
      {
         var oLoginWhere = from Logins in oContext.Logins
                           where Logins.idUser == oLogin.idUser
                           select Logins.idRow;
         oQuery = oQuery.Where(DATA => oLoginWhere.Contains(DATA.CreatedBy.Value));
       }
      #endregion

      #region ApplySave

      protected bool ApplySave<tModel>(tModel oData, Model.Login oLogin, List<Message> oMSG, Action<tModel> AfterSaveCallback) where tModel : Model.Base
      { return this.ApplySave<tModel>(oData, oLogin, oMSG, AfterSaveCallback, true); }

      protected bool ApplySave<tModel>(tModel oData, Model.Login oLogin, List<Message> oMSG, Action<tModel> AfterSaveCallback, bool UseAuditTrail) where tModel : Model.Base
      {
         bool bReturn = false;

         try
         {

            // TRANSACTION
            //this.Context.Database.Connection.BeginTransaction();

            // ORIGINAL
            var idORIGINAL = oData.idRow; long idOLD = 0;

            // OLD RECORD
            if (idORIGINAL > 0 && UseAuditTrail == true && this.AuditTrailActivated == true)
            {
               using (var oOLD = this.Context.Set<tModel>().Where(DATA => DATA.idRow == idORIGINAL).SingleOrDefault())
               {
                  if (oOLD != null)
                  {

                     this.Context.ObjectContext.Detach(oOLD);
                     oOLD.RowStatus = Model.Base.enRowStatus.Removed;
                     oOLD.RemovedBy = oLogin.idRow;
                     oOLD.RemovedIn = DateTime.Now;
                     oOLD.idRow = 0;
                     this.Context.Set<tModel>().Attach(oOLD);
                     this.Context.Entry<tModel>(oOLD).State = System.Data.Entity.EntityState.Added;
                     if (this.Context.SaveChanges(oMSG) == false) { return bReturn; }
                     idOLD = oOLD.idRow;
                  }
               }
             }

            // NEW RECORD
            var iState = System.Data.Entity.EntityState.Added;
            if (idORIGINAL != 0) { iState = System.Data.Entity.EntityState.Modified; }
            oData.idOriginal = idOLD;
            oData.RowStatus = Model.Base.enRowStatus.Active;
            oData.CreatedIn = DateTime.Now;
            if (oLogin != null && oLogin.idRow > 0) { oData.CreatedBy = oLogin.idRow; }
            this.Context.Set<tModel>().Attach(oData);
            this.Context.Entry<tModel>(oData).State = iState;
            if (this.Context.SaveChanges(oMSG) == false) { return bReturn; }

            // AFTER SAVE CALLBACK
            if (idORIGINAL == 0 && AfterSaveCallback != null)
            {
               AfterSaveCallback(oData); 
               if (this.Context.SaveChanges(oMSG) == false) { return bReturn; }
             }

            // OK
            bReturn = true;

          }
         catch (Exception ex) { oMSG.Add(new Message() { Exception = ex.Message }); }

         return bReturn;
       }

      #endregion

      #region ApplyRemove
      protected bool ApplyRemove<tModel>(tModel oData, Model.Login oLogin, List<Message> oMSG) where tModel : Model.Base
      {
         bool bReturn = false;

         try
         {

            // TRANSACTION
            //this.Context.Database.Connection.BeginTransaction();

            // SET DATA
            /*
            oData.RowStatus = Model.Base.enRowStatus.Removed;
            oData.RemovedBy = oLogin.idRow;
            oData.RemovedIn = DateTime.Now;
            this.Context.Set<tModel>().Attach(oData);
            this.Context.Entry<tModel>(oData).State = System.Data.Entity.EntityState.Modified;
            */
            var oRemove = this.Context.Set<tModel>().Where(DATA => DATA.idRow == oData.idRow).SingleOrDefault();
            oRemove.RowStatus = Model.Base.enRowStatus.Removed;
            oRemove.RemovedBy = oLogin.idRow;
            oRemove.RemovedIn = DateTime.Now;

            // SAVE
            if (this.Context.SaveChanges(oMSG) == false) { return bReturn; }

            // OK
            bReturn = true;

         }
         catch (Exception ex) { oMSG.Add(new Message() { Exception = ex.Message }); }

         return bReturn;
      }
      #endregion

      #region ModelSet
      protected IQueryable<tMODEL> ModelSet<tMODEL>(Model.Login oLogin) where tMODEL: Model.Base
      {
         var oLoginWhere = from Logins in oContext.Logins
                           where Logins.idUser == oLogin.idUser
                           select Logins.idRow;

         return this.Context.Set<tMODEL>().Where(DATA => oLoginWhere.Contains(DATA.CreatedBy.Value));

      }
      #endregion

      #region ApplyPagination

      protected void ApplyPagination<S, M>(ref IQueryable<M> oQuery, Parameters oParameters, ref Return oReturn) where S : Service.Base where M: Model.Base
      {

         // CHECK CONFIG
         if (oParameters.DATA.ContainsKey("DoesntApplyPagination") && oParameters.DATA["DoesntApplyPagination"] != null && ((bool)oParameters.DATA["DoesntApplyPagination"]) == true)
         { return; }

         // PAGE SIZE
         Int16 iPageSize = 10;
         string sPageSize = System.Configuration.ConfigurationManager.AppSettings["PageSize"];
         if (!string.IsNullOrEmpty(sPageSize))
         {
            Int16.TryParse(sPageSize, out iPageSize);
          } 

         // ANALYZE PAGINATION
         Int16 iPageCurrent = 1; if (oParameters.DATA.ContainsKey("Page") && oParameters.DATA["Page"] != null && ((Int16?)oParameters.DATA["Page"]).Value != 0) { iPageCurrent = ((Int16)oParameters.DATA["Page"]); }
         Int16? iNextPage = iPageCurrent; iNextPage += 1;
         int? iRowsSkip = ((iPageCurrent - 1) * iPageSize);
         oReturn.DATA.Add("NextPage", iNextPage);

         // HAS MORE PAGES
         int iCount = oQuery.Count();
         bool bHasMorePages = (iCount > (iPageCurrent * iPageSize) ? true : false);
         oReturn.DATA.Add("HasMorePages", bHasMorePages);

         // TOTAL PAGES
         int iTotalPages = (int)(iCount / iPageSize);
         if (iCount % iPageSize > 0) { iTotalPages++; }
         oReturn.DATA.Add("Pages", this.ApplyPagination_GetPages(iTotalPages, iPageCurrent));

         // APPLY PAGINATION
         oQuery = oQuery.Skip(iRowsSkip.Value).Take(iPageSize);

       }

      private List<Pages> ApplyPagination_GetPages(int iTotalPages, Int16 iPageCurrent)
      {
         var oPages = new List<Pages>();
         Int16 iGroup = 0;

         // LESS THEN TEN PAGES TO RETURN
         if (iTotalPages <= 7)
         {
            for (Int16 iPage = 1; iPage <= iTotalPages; iPage++)
            {
               this.ApplyPagination_GetPages_Add(ref oPages, ref iGroup, iPage, iPageCurrent);
            }
         }

         // MORE THEN TEN PAGES TO RETURN
         if (iTotalPages > 7)
         {

            // add first three
            for (Int16 iPage = 1; iPage <= 1; iPage++)
            {
               this.ApplyPagination_GetPages_Add(ref oPages, ref iGroup, iPage, iPageCurrent);
            }

            // add middle pages
            Int32 iStartingPage = (iPageCurrent - 2);
            Int32 iEndingPage = (iStartingPage + 4);
            iStartingPage = (iStartingPage <= 0 ? 1 : iStartingPage);
            iEndingPage = (iEndingPage >= iTotalPages ? iTotalPages : iEndingPage);
            for (Int32 iPage = iStartingPage; iPage <= iEndingPage; iPage++)
            {
               this.ApplyPagination_GetPages_Add(ref oPages, ref iGroup, Convert.ToInt16(iPage), iPageCurrent);
            }

            //add last three
            for (Int32 iPage = (iTotalPages - 0); iPage <= iTotalPages; iPage++)
            {
               this.ApplyPagination_GetPages_Add(ref oPages, ref iGroup, Convert.ToInt16(iPage), iPageCurrent);
            }

         }

         return oPages;
       }

      private void ApplyPagination_GetPages_Add(ref List<Pages> oPages, ref Int16 iGroup, Int16 iPage, Int16 iPageCurrent)
      {
         if (oPages.Count(DATA => DATA.Page == iPage) == 0) 
         {
            if (oPages.Count(DATA => DATA.Page == (iPage - 1)) == 0) { iGroup++; }
            Pages oNew = new Pages(iGroup, iPage);
            if (iPageCurrent == iPage) { oNew.IsCurrent = true; }
            oPages.Add(oNew); 
          }
       }

      #endregion

      #region Dispose
      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
       }
      protected virtual void Dispose(bool disposing)
      {
         if (disposing)
         {

            /*
            if (managedResource != null)
            {
               managedResource.Dispose();
               managedResource = null;
            }
            */
         }

         /* 
         if (nativeResource != IntPtr.Zero)
         {
            Marshal.FreeHGlobal(nativeResource);
            nativeResource = IntPtr.Zero;
         }
         */

         if (this.oContext != null)
         {
            if (this.oContext.Database != null && this.oContext.Database.Connection != null && this.oContext.Database.Connection.State != System.Data.ConnectionState.Closed)
            {
               this.oContext.Database.Connection.Close();
            }
            this.oContext = null;
          }

      }
      #endregion

   }
}
