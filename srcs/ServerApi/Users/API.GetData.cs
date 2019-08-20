using System.Linq;

namespace FriendlyCashFlow.API.Users
{
   partial class UsersService
   {

      private IQueryable<UserData> GetDataQuery()
      {
         return this.dbContext.Users
            .Where(x => x.RowStatus == (short)Base.enRowStatus.Active)
            .AsQueryable();
      }

   }
}
