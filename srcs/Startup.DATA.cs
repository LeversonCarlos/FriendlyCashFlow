using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FriendlyCashFlow
{
   partial class Startup
   {

      private void AddDataServices(IServiceCollection services)
      {
         // services.AddDbContext<Context>(x => x.UseInMemoryDatabase("TestDb"));
      }

   }
}
