#region Using
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace FriendCash.Service.Imports.Model
{
   public enum enImportState : short { Initializing = 0, Importing = 1, Finished = 2, Canceled = 9 }

   [Table("v5_dataImports")]
   public class bindImport : Base.BaseModel
   {

      #region New
      public bindImport()
      {
         this.idImport = -1;
      }
      #endregion

      #region idImport
      [Column("idImport"), Key]
      [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
      public long idImport { get; set; }
      #endregion

      #region idUser
      [Column("idUser", TypeName = "varchar")]
      [StringLength(128), Required]
      public string idUser { get; set; }
      #endregion

      #region Accounts

      [Column("TotalAccounts")]
      public int TotalAccounts { get; set; }

      [Column("ImportedAccounts")]
      public int ImportedAccounts { get; set; }

      #endregion

      #region Categories

      [Column("TotalCategories")]
      public int TotalCategories { get; set; }

      [Column("ImportedCategories")]
      public int ImportedCategories { get; set; }

      #endregion

      #region Entries

      [Column("TotalEntries")]
      public int TotalEntries { get; set; }

      [Column("ImportedEntries")]
      public int ImportedEntries { get; set; }

      #endregion

      #region Message
      [Column("Message", TypeName = "varchar")]
      [StringLength(4000)]
      public string Message { get; set; }
      #endregion

      #region State

      [Column("State"), Required]
      public short StateValue { get; set; }

      [NotMapped]
      public enImportState State
      {
         get { return ((enImportState)this.StateValue); }
         set { this.StateValue = ((short)value); }
      }

      #endregion

   }

}
