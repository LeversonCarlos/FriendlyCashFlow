using System;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Base
{
   internal partial class dbContext : DbContext, IDisposable
   {

      public IServiceProvider serviceProvider { get; set; }
      public dbContext(DbContextOptions<dbContext> options, IServiceProvider _serviceProvider) : base(options)
      { this.serviceProvider = _serviceProvider; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         this.OnModelCreating_Users(modelBuilder);
         this.OnModelCreating_UserResources(modelBuilder);
         this.OnModelCreating_UserRoles(modelBuilder);
         this.OnModelCreating_Accounts(modelBuilder);
         this.OnModelCreating_Categories(modelBuilder);
         base.OnModelCreating(modelBuilder);
      }

      /*
         dotnet ef migrations add Initial
         dotnet ef database update
       */

   }
}
