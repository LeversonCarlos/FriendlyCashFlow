using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FriendCash.Service
{

   /* 
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
   */ 

   #region Pages
   public class Pages
   {
      internal Pages(Int16 iGroup, Int16 iPage) { this.Group = iGroup; this.Page = iPage; }
      public Int16 Group { get; set; }
      public Int16 Page { get; set; }
      public bool IsCurrent { get; set; }
   }
   #endregion

   /*
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

      #region GetDateData
      public DateTime? GetDateData(string Key)
      {
         DateTime? oReturn = null;
         if (this.DATA.ContainsKey(Key) && this.DATA[Key] != null)
         {
            try { oReturn = ((DateTime)this.DATA[Key]); }
            catch { }
         }
         return oReturn;
      }
      #endregion

   }
   #endregion
   */ 

}
