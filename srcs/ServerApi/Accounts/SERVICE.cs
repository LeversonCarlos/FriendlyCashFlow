
using System;

namespace FriendlyCashFlow.API.Accounts
{
   internal partial class AccountsService : Base.BaseService
   {
      private const string resourceID = "{ResourceID}";
      public AccountsService(IServiceProvider serviceProvider) : base(serviceProvider) { }
   }
}
