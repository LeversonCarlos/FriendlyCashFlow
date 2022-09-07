using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Lewio.CashFlow.Accounts;

[Table("v7_Accounts")]
internal class AccountEntity
{

   [Key]
   [Column(TypeName = "varchar(36)"), StringLength(36)]
   public string AccountID { get; set; }

   [Column(TypeName = "varchar(36)"), StringLength(36), Required]
   public string UserID { get; set; }

   [Column(TypeName = "varchar(500)"), StringLength(500), Required]
   public string Text { get; set; }

   public short Type { get; set; }

   public short? ClosingDay { get; set; }
   public short? DueDay { get; set; }

   public bool Active { get; set; }

}
