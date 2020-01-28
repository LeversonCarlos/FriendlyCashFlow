﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Import
{

   public class ApiClient : HttpClient
   {

      private ApiToken Token { get; set; }
      private readonly AppSettings_Api apiSettings;
      public ApiClient(AppSettings_Api apiSettings)
      {
         this.apiSettings = apiSettings;
      }

      public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
      {
         if (this.Token != null)
         {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Berear", this.Token.AccessToken);
         }
         return base.SendAsync(request, cancellationToken);
      }

      public async Task<bool> AuthAsync()
      {
         try
         {
            Console.WriteLine(" Authenticating on api with the provided credentials");
            using (var httpClient = new HttpClient())
            {
               var authData = new
               {
                  UserName = this.apiSettings.Username,
                  Password = this.apiSettings.Password,
                  GrantType = "password"
               };
               var authDataJson = System.Text.Json.JsonSerializer.Serialize(authData);
               var authDataContent = new StringContent(authDataJson, Encoding.UTF8, "application/json");
               var authMessage = await httpClient.PostAsync($"{this.apiSettings.Url}/api/users/auth", authDataContent);
               var authResultContent = await authMessage.Content.ReadAsStringAsync();
               if (!authMessage.IsSuccessStatusCode) { Console.WriteLine($" AuthResult: {authResultContent}"); }
               this.Token = System.Text.Json.JsonSerializer.Deserialize<ApiToken>(authResultContent);
               if (this.Token == null) { Console.WriteLine(" Could not authenticate on api"); return false; }
               Console.WriteLine($" UserID: {this.Token.UserID}");
               return true;
            }
         }
         catch (Exception ex) { Console.WriteLine($" Exception: {ex.Message}"); return false; }
      }

   }

   public class ApiToken
   {
      public string UserID { get; set; }
      public string AccessToken { get; set; }
      public string RefreshToken { get; set; }
   }

}
