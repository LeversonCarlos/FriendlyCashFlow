using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendCash.Model.Tools
{

   public class Return : Package { }
   public class Parameters : Package { }

   public class Package
   {

      #region  NEW
      public Package(List<Message> oMSGs) : this() { this.oMSG = oMSGs; }
      public Package() { this.OK = false; }
      #endregion

      #region OK
      public bool OK { get; set; }
      #endregion

      #region LOGIN
      public Model.Login Login { get; set; }
      #endregion

      #region MSG
      private List<Message> oMSG = new List<Message>();
      public List<Message> MSG
      {
         get { return this.oMSG; }
      }
      #endregion
      
      #region DATA

      private SortedList<string, object> oDATA = new SortedList<string, object>();
      public SortedList<string, object> DATA
      {
         get { return this.oDATA; }
      }

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

      #endregion

   }
}
