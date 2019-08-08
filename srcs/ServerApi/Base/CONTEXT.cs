using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendlyCashFlow.API.Base
{
   internal partial class dbContext : DbContext, IDisposable
   {

      public IServiceProvider serviceProvider { get; set; }
      public dbContext(DbContextOptions<dbContext> options, IServiceProvider _serviceProvider) : base(options)
      { this.serviceProvider = _serviceProvider; }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
         optionsBuilder
            .ReplaceService<IHistoryRepository, MigrationsHistory>();
         base.OnConfiguring(optionsBuilder);
      }

      /*
         dotnet ef migrations add Initial
         dotnet ef database update
       */

   }
}
