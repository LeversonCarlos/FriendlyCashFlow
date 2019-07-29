using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FriendlyCashFlow
{
   partial class Startup
   {

      private void AddDataServices(IServiceCollection services)
      {
         services.AddDbContext<API.Shared.dbContext>(x => x.UseInMemoryDatabase("FriendlyCashFlowDb"));
      }

   }
}
