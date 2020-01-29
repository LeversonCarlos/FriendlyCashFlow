using System;
using System.Collections.Generic;
using System.Linq;
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
         this.BaseAddress = new Uri(this.apiSettings.Url);
      }

      public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
      {
         if (this.Token != null)
         {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this.Token.AccessToken);
         }
         return base.SendAsync(request, cancellationToken);
      }

      public async Task<bool> AuthAsync()
      {
         try
         {
            Console.WriteLine(" Info: Authenticating on api with the provided credentials");
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
               if (this.Token == null) { Console.WriteLine(" Warning: Could not authenticate on api"); return false; }
               Console.WriteLine($" UserID: {this.Token.UserID}");
               return true;
            }
         }
         catch (Exception ex) { Console.WriteLine($" Exception: {ex.Message}"); return false; }
      }

      public async Task<bool> AuthAsync(string refreshToken)
      {
         try
         {
            Console.WriteLine(" Info: Refreshing Token");
            using (var httpClient = new HttpClient())
            {
               var authData = new
               {
                  RefreshToken = refreshToken,
                  GrantType = "refresh_token"
               };
               var authDataJson = System.Text.Json.JsonSerializer.Serialize(authData);
               var authDataContent = new StringContent(authDataJson, Encoding.UTF8, "application/json");
               var authMessage = await httpClient.PostAsync($"{this.apiSettings.Url}/api/users/auth", authDataContent);
               var authResultContent = await authMessage.Content.ReadAsStringAsync();
               if (!authMessage.IsSuccessStatusCode) { Console.WriteLine($" AuthResult: {authResultContent}"); }
               this.Token = System.Text.Json.JsonSerializer.Deserialize<ApiToken>(authResultContent);
               if (this.Token == null) { Console.WriteLine(" Warning: Could not authenticate on api"); return false; }
               return true;
            }
         }
         catch (Exception ex) { Console.WriteLine($" Exception: {ex.Message}"); return false; }
      }

      public async Task<bool> ImportAsync(List<Entry> entries, List<Transfer> transfers)
      {
         try
         {
            Console.WriteLine(" Info: Sending data to API");
            if (entries == null) { entries = new List<Entry>(); }
            if (transfers == null) { transfers = new List<Transfer>(); }

            // YEAR INTERVAL
            var yearList = entries
               .Select(x => x.DueDate.Year).ToList()
               .Union(transfers.Select(x => x.Date.Year).ToList())
               .GroupBy(x => x)
               .Select(x => x.Key)
               .OrderBy(x => x)
               .ToList();

            foreach (var year in yearList)
            {
               var importResult = await this.ImportAsync(
                  entries.Where(x => x.DueDate.Year == year).ToList(),
                  transfers.Where(x => x.Date.Year == year).ToList(),
                  year, (year == yearList[0]));
               if (!importResult) { return false; }
            }

            return true;
         }
         catch (Exception ex) { Console.WriteLine($" Exception: {ex.ToString()}"); return false; }
      }

      public async Task<bool> ImportAsync(List<Entry> entries, List<Transfer> transfers, int year, bool clearDataBefore)
      {
         try
         {
            var startTime = DateTime.Now;
            Console.Write($" Year: {year}");
            Console.Write(" - waiting");

            var importParam = new
            {
               ClearDataBefore = clearDataBefore,
               Entries = entries,
               Transfers = transfers
            };
            var importParamJson = System.Text.Json.JsonSerializer.Serialize(importParam);
            var importParamContent = new StringContent(importParamJson, Encoding.UTF8, "application/json");

            var importMessage = await this.PostAsync("api/import", importParamContent);
            var importContent = await importMessage.Content.ReadAsStringAsync();
            var finishTime = DateTime.Now;
            Console.Write($" - {Math.Round(finishTime.Subtract(startTime).TotalSeconds, 0)} sec");

            if (!importMessage.IsSuccessStatusCode)
            {
               Console.Write($" - Status:{importMessage.StatusCode}");
               if (!string.IsNullOrEmpty(importContent)) { Console.Write($" - Content:{importContent}"); }

               if (importMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
               {
                  Console.WriteLine("");
                  if (await this.AuthAsync(this.Token.RefreshToken))
                  {
                     return await this.ImportAsync(entries, transfers, year, clearDataBefore);
                  }
               }

               Console.WriteLine("");
               return false;
            }

            Console.WriteLine($" - OK");
            return true;
         }
         catch (Exception) { Console.WriteLine(""); throw; }
      }

   }

   public class ApiToken
   {
      public string UserID { get; set; }
      public string AccessToken { get; set; }
      public string RefreshToken { get; set; }
   }

}
