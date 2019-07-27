using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriendCash.Web.Code
{

   #region CommandBase
   public class CommandBase
   {

      #region New
      public CommandBase()
      { this.ButtonType = enButtonType.Button; }
      #endregion

      #region Name
      public string Name { get; set; }
      public string GetName()
      {
         return (string.IsNullOrEmpty(this.Name) ? "" : "id=" + this.Name + "");
      }
      #endregion

      #region Text
      public string Text { get; set; }
      #endregion

      #region Color
      public enum enColor { Default = 0, Primary, Success, Info, Warning, Danger };
      public enColor Color { get; set; }
      public string GetColor()
      {
         return "btn-" + this.Color.ToString().ToLower();
      }
      #endregion

      #region Icon
      public enum enIcon { None = 0, Cancel, Save, Remove, Add, Upload, User };
      public enIcon Icon { get; set; }
      public string GetIcon()
      {
         var sReturn = string.Empty;
         switch (this.Icon)
         {
            case enIcon.Cancel: sReturn = "remove"; break;
            case enIcon.Save: sReturn = "floppy-disk"; break;
            case enIcon.Remove: sReturn = "trash"; break;
            case enIcon.Add: sReturn = "plus"; break;
            case enIcon.Upload: sReturn = "cloud-upload"; break;
            case enIcon.User: sReturn = "user"; break;
         }
         return sReturn;
      }
      #endregion

      #region ButtonType
      public enum enButtonType { Button = 0, Submit, Link };
      public enButtonType ButtonType { get; set; }
      public string GetButtonType()
      {
         return (this.ButtonType == enButtonType.Link ? "" : "Type=" + this.ButtonType.ToString().ToLower() + "");
      }
      /*
      public string GetElement()
      { return (this.Type == enType.Link ? "a" : "button"); }
      */ 
      #endregion

      #region Click
      public string Click { get; set; }
      public string GetClick()
      {
         return (string.IsNullOrEmpty(this.Click) ? "" : "onclick=" + this.Click + "");
      }
      #endregion

      #region CSS
      public string CSS { get; set; }
      #endregion

      #region Data
      public string Data { get; set; }
      #endregion

   }
   #endregion

   #region CommandSave
   public class CommandSave : CommandBase
   {

      #region New
      public CommandSave()
      {
         this.Name = "SaveButton";
         this.Text = FriendCash.Resources.Base.COMMAND_SAVE;
         this.Color = enColor.Success;
         this.Icon = enIcon.Save;
         this.UseCancel = true;
         this.UseSubmit = false;
         this.UsePopup = false;
         this.CSS = "SaveButtonHooked"; 
       }
      #endregion

      #region UseCancel
      public bool UseCancel { get; set; }
      #endregion

      #region UseSubmit
      private bool bUseSubmit = false;
      public bool UseSubmit 
      { 
         get { return this.bUseSubmit; } 
         set 
         { this.bUseSubmit = value;
         if (this.bUseSubmit) { this.CSS = ""; this.ButtonType = enButtonType.Submit; } 
          } 
      }
      #endregion

      #region UsePopup
      public bool UsePopup { get; set; }
      #endregion

   }
   #endregion

   #region CommandRemove
   public class CommandRemove : CommandBase
   {

      #region New
      public CommandRemove()
      {
         this.Name = "RemoveButton";
         this.Text = FriendCash.Resources.Base.COMMAND_REMOVE;
         this.Color = enColor.Danger;
         this.Icon = enIcon.Remove;
      }
      #endregion

      public string Action { get; set; }
      public string Question { get; set; }

   }
   #endregion

   #region CommandCreate
   public class CommandCreate : CommandBase
   {

      #region New
      public CommandCreate()
      {
         this.Text = FriendCash.Resources.Base.COMMAND_CREATE;
         this.Color = enColor.Primary;
         this.Icon = enIcon.Add;
      }
      #endregion

      #region Action
      private string sAction = string.Empty;
      public string Action
      {
         get { return sAction; }
         set 
         { 
            sAction = value;  
            this.Click = "";
            if (!string.IsNullOrEmpty(sAction)) { this.Click = "window.location='" + sAction + "'"; }
         }
       }
      #endregion

      #region UsePopup
      public bool UsePopup { get; set; }
      #endregion

   }
   #endregion

}