using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FriendCash.Service
{

   #region Base
   public abstract class Base: IDisposable
   {

      #region Context
      private Model.Context oContext = null;
      public Model.Context Context 
      {
         get
         {
            if (this.oContext == null)
            {
               this.oContext = new Model.Context();
             }
            return this.oContext;
          }
       }
      #endregion

      #region GetOriginalID
      protected long GetOriginalID(Model.Base oBase)
      {
         long iReturn = 0;
         if (oBase.idRow != null && oBase.idRow > 0)
         {
            iReturn = oBase.idRow;
         }
         return iReturn;
       }
      #endregion

      #region ApplyDataToNew
      protected void ApplyDataToNew(Model.Base oBase, Model.Login oLogin, long idOriginal)
      {
         if (oLogin != null && oLogin.idRow > 0)
         {
            oBase.CreatedBy = oLogin.idRow;
         }
         oBase.CreatedIn = DateTime.Now;
         oBase.RowStatus = Model.Base.enRowStatus.Active;
         if (idOriginal > 0)
         {
            oBase.idOriginal = idOriginal;
         }
      }
      #endregion

      #region ApplyDataToOld
      protected void ApplyDataToOld(Model.Base oBase, Model.Login oLogin)
      {
         oBase.RowStatus = Model.Base.enRowStatus.Removed;
         oBase.RemovedBy = oLogin.idRow;
         oBase.RemovedIn = DateTime.Now;
       }
      #endregion

      #region ApplyPagination
      protected void ApplyPagination<S, M>(ref IQueryable<M> oQuery, Parameters oParameters, ref Return oReturn) where S : Service.Base where M: Model.Base
      {

         // ANALYZE PAGINATION
         Int16 iPageSize = 10;
         Int16? iPageCurrent = 1; if (oParameters.DATA.ContainsKey("Page") && oParameters.DATA["Page"] != null && ((Int16?)oParameters.DATA["Page"]).Value != 0) { iPageCurrent = ((Int16?)oParameters.DATA["Page"]); }
         Int16? iNextPage = iPageCurrent; iNextPage += 1;
         int? iRowsSkip = ((iPageCurrent - 1) * iPageSize);
         oReturn.DATA.Add("NextPage", iNextPage);

         // HAS MORE PAGES
         bool bHasMorePages = (oQuery.Count() > (iPageCurrent * iPageSize) ? true : false);
         oReturn.DATA.Add("HasMorePages", bHasMorePages);

         // APPLY PAGINATION
         oQuery = oQuery.Skip(iRowsSkip.Value).Take(iPageSize);

       }
      #endregion

      #region Fields

      public delegate void GetEntityNameHandler(ref string Value);
      public event GetEntityNameHandler GetEntityName;

      private FieldsClass oFields = null;
      public FieldsClass Fields
      {
         get
         {
            if (this.oFields == null && this.GetEntityName != null)
            {
               string sEntityName = string.Empty;
               this.GetEntityName(ref sEntityName);
               if (!string.IsNullOrEmpty(sEntityName))
               {
                  this.oFields = new FieldsClass(sEntityName);
                }
            }
            return this.oFields;
          }
       }

      public class FieldsClass
      {

         internal FieldsClass(string sEntity) { this.Entity= sEntity; }
         public string ID { get { return "idRow"; } }
         public string Search { get { return "Search"; } }
         public string Entity { get; private set; }
         public string Key { get { return "id" + this.Entity; } }
         public string List { get { return this.Entity + "s"; } }

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
            this.oContext = null;
          }

      }
      #endregion

   }
   #endregion

   #region Data
   public abstract class Data
   {

      #region DATA
      private SortedList<string, object> oDATA = new SortedList<string, object>();
      public SortedList<string, object> DATA
      {
         get { return this.oDATA; }
      }
      #endregion

      #region GetNumericData
      public long GetNumericData(string Key)
      {
         long iReturn = 0;
         if (this.DATA.ContainsKey(Key) && this.DATA[Key] != null)
         {
            iReturn = ((long)this.DATA[Key]);
          }
         return iReturn;
       }
      #endregion

      #region GetTextData
      public string GetTextData(string Key)
      {
         string sReturn = string.Empty;
         if (this.DATA.ContainsKey(Key) && this.DATA[Key] != null)
         {
            sReturn = this.DATA[Key].ToString();
         }
         return sReturn;
      }
      #endregion

    }
   #endregion

   #region Parameters
   public class Parameters : Data
   {

      #region Login
      public Model.Login Login { get; set; }
      #endregion

   }
   #endregion

   #region Return
   public class Return : Data
   {
      public Return() { this.OK = false; }
      public bool OK { get; set; }

      #region MSG
      private List<string> oMSG = new List<string>();
      public List<string> MSG 
      {
         get { return this.oMSG; }
       }
      #endregion

    }
   #endregion

}
