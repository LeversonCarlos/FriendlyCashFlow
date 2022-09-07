using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Lewio.CashFlow.Common;

internal class BaseEntity
{

   [Column(TypeName = "varchar(36)"), StringLength(36), Required]
   public string UserID { get; set; }

}
