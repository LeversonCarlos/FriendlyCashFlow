using System;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow
{

   namespace API.Base
   {

      internal partial class dbContext : DbContext, IDisposable
      {

         public dbContext(DbContextOptions<dbContext> options) : base(options)
         { }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
            this.OnModelCreating_Users(modelBuilder);
            this.OnModelCreating_UserResources(modelBuilder);
            this.OnModelCreating_UserRoles(modelBuilder);
            this.OnModelCreating_UserToken(modelBuilder);
            this.OnModelCreating_Accounts(modelBuilder);
            this.OnModelCreating_Categories(modelBuilder);
            this.OnModelCreating_Balances(modelBuilder);
            this.OnModelCreating_Patterns(modelBuilder);
            this.OnModelCreating_Recurrencies(modelBuilder);
            this.OnModelCreating_Entries(modelBuilder);
            this.OnModelCreating_Budget(modelBuilder);
            base.OnModelCreating(modelBuilder);
         }

         /*
            dotnet ef migrations add Initial
            dotnet ef database update
          */

         internal static dbContext CreateNewContext(string connStr)
         {
            var builder = new DbContextOptionsBuilder<dbContext>();
            builder.ConfigureSqlServer(connStr);
            var ctx = new dbContext(builder.Options);
            return ctx;
         }

      }

   }

   internal static class dbContextExtensions
   {
      public static DbContextOptionsBuilder ConfigureSqlServer(this DbContextOptionsBuilder self, string connStr)
      {
         self.UseSqlServer(connStr, opt =>
         {
            opt.MigrationsHistoryTable("v6_MigrationsHistory");
         });
         return self;
      }

   }

}
