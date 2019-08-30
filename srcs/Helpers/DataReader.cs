using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FriendlyCashFlow.Helpers
{
   internal class DataReader : IDisposable
   {

      private readonly IOptions<AppSettings> _appSettings;
      public DataReader([FromServices] IOptions<AppSettings> appSettings) { this._appSettings = appSettings; }

      public void Dispose()
      {
         throw new NotImplementedException();
      }

   }
}
