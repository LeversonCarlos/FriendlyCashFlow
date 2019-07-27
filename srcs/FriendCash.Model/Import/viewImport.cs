#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Service.Imports.Model
{
   public class viewImport
   {

      #region New
      public viewImport() { }
      public viewImport(bindImport Value) : this()
      {
         this.idImport = Value.idImport;
         this.TotalAccounts = Value.TotalAccounts;
         this.ImportedAccounts = Value.ImportedAccounts;
         this.TotalCategories = Value.TotalCategories;
         this.ImportedCategories = Value.ImportedCategories;
         this.TotalEntries = Value.TotalEntries;
         this.ImportedEntries = Value.ImportedEntries;
         this.State = Value.State;
         this.Message = Value.Message;
         this.RowDate = Value.RowDate;
      }
      #endregion

      #region idImport
      [Display(Description = "LABEL_IMPORTS_IDIMPORT")]
      public long idImport { get; set; }
      #endregion

      #region Accounts 

      [Display(Description = "LABEL_IMPORTS_ACCOUNTS")]
      public short Accounts
      {
         get
         {
            if (this.TotalAccounts <= 0 || this.ImportedAccounts <= 0) { return 0; }
            else {
               return (short)(((double)(this.ImportedAccounts)) / ((double)(this.TotalAccounts)) * (double)100);
            }
         }
      }

      public int TotalAccounts { get; set; }
      public int ImportedAccounts { get; set; }

      #endregion

      #region Categories 

      [Display(Description = "LABEL_IMPORTS_CATEGORIES")]
      public short Categories       
      {
         get
         {
            if (this.TotalCategories <= 0 || this.ImportedCategories <= 0) { return 0; }
            else {
               return (short)(((double)(this.ImportedCategories)) / ((double)(this.TotalCategories)) * (double)100);
            }
         }
      }

      public int TotalCategories { get; set; }
      public int ImportedCategories { get; set; }

      #endregion

      #region Entries 

      [Display(Description = "LABEL_IMPORTS_ENTRIES")]
      public short Entries 
      {
         get
         {
            if (this.TotalEntries <= 0 || this.ImportedEntries <= 0) { return 0; }
            else {
               return (short)(((double)(this.ImportedEntries)) / ((double)(this.TotalEntries)) * (double)100);
            }
         }
      }

      public int TotalEntries { get; set; }
      public int ImportedEntries { get; set; }

      #endregion

      #region State
      public short StateValue { get; set; }
      [Display(Description = "LABEL_IMPORTS_STATE")]
      public enImportState State
      {
         get { return ((enImportState)this.StateValue); }
         set { this.StateValue = ((short)value); }
      }
      #endregion

      #region Message
      [Display(Description = "LABEL_IMPORTS_MESSAGE")]
      public string Message { get; set; }
      #endregion

      #region RowDate
      [Display(Description = "LABEL_IMPORTS_DATE")]
      public DateTime RowDate { get; set; }
      #endregion

   }
}