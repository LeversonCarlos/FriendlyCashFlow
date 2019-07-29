using System;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Shared
{
   internal partial class dbContext : DbContext, IDisposable
   {

      public IServiceProvider serviceProvider { get; set; }
      public dbContext(DbContextOptions<dbContext> options, IServiceProvider _serviceProvider) : base(options)
      { this.serviceProvider = _serviceProvider; }

   }
}
