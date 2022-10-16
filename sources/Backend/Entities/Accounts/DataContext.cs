using Lewio.CashFlow.Accounts;
using Microsoft.EntityFrameworkCore;
namespace Lewio.CashFlow.Common;

partial class DataContext
{

   internal DbSet<AccountEntity> Accounts { get; set; }

   private void OnModelCreating_Accounts(ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<AccountEntity>()
         .HasIndex(x => new { x.UserID, x.AccountID, x.Text });
   }

}
