using System;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Base
{
   internal partial class dbContext : DbContext, IDisposable
   {

      public IServiceProvider serviceProvider { get; set; }
      public dbContext(DbContextOptions<dbContext> options, IServiceProvider _serviceProvider) : base(options)
      { this.serviceProvider = _serviceProvider; }

      /*
         dotnet ef migrations add Initial
         dotnet ef database update
       */

   }
}
