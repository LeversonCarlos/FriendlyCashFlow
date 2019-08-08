using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;

namespace FriendlyCashFlow.API.Base
{
   public class MigrationsHistory : SqlServerHistoryRepository
   {
      public MigrationsHistory(HistoryRepositoryDependencies dependencies) : base(dependencies) { }

      protected override void ConfigureTable(EntityTypeBuilder<HistoryRow> history)
      {
         base.ConfigureTable(history);
         history.ToTable("v6_MigrationsHistory");
         // history.Property(h => h.MigrationId).HasColumnName("migration_id");
         // history.Property(h => h.ProductVersion).HasColumnName("product_version");
      }
   }
}
