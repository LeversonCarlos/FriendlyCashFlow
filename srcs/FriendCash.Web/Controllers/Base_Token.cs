#region Using
using FriendCash.Service.Base;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
#endregion

namespace FriendCash.Web.Controllers
{
   partial class Base
   {

      #region TokenApply

      internal async Task<Bundle<System.Net.HttpStatusCode>> TokenApply(string username, string password, bool isPersistent, IAuthenticationManager authManager)
      {
         var bundleResult = new Bundle<System.Net.HttpStatusCode>();
         try
         {

            // LOAD
            var tokenResult = await this.TokenLoad(username, password);
            if (tokenResult.Result == false && tokenResult.Messages.Count != 0)
            {
               /*
               bundleResult.Messages.AddRange(tokenResult.Messages);
               var statusCode = tokenResult.Messages
                  .Where(x => x.Text.Contains("StatusCode:"))
                  .Select(x => x.Text.Replace("StatusCode:", ""))
                  .Where(x => !string.IsNullOrEmpty(x))
                  .Select(x => int.Parse(x))
                  .FirstOrDefault();
               bundleResult.Data = (System.Net.HttpStatusCode)statusCode;
               return bundleResult;
               */
               bundleResult.Messages.Add(new BundleMessage(this.GetTranslationByKey("MSG_AUTH_INVALID_LOGIN")));
               bundleResult.Data = System.Net.HttpStatusCode.Forbidden;
               return bundleResult;
            }
            else if (tokenResult.Data == null || string.IsNullOrEmpty(tokenResult.Data.access_token))
            {
               bundleResult.Messages.Add(new BundleMessage(this.GetTranslationByKey("MSG_AUTH_INVALID_LOGIN")));
               bundleResult.Data = System.Net.HttpStatusCode.Forbidden;
               return bundleResult;
            }

            // APPLY
            bundleResult = await this.TokenApply(username, isPersistent, tokenResult.Data, authManager);
            return bundleResult;

         }
         catch (Exception ex) { bundleResult.Messages.Add(new BundleMessage(ex.ToString(), BundleMessage.enumType.Alert)); }
         return bundleResult;
      }

      internal async Task<Bundle<System.Net.HttpStatusCode>> TokenApply(string refreshToken, bool isPersistent)
      {
         var bundleResult = new Bundle<System.Net.HttpStatusCode>();
         try
         {

            // AUTHENTICATION
            var authManager = this.HttpContext.GetOwinContext().Authentication;

            // LOAD
            var tokenResult = await this.TokenLoad(refreshToken);
            if (tokenResult.Result == false && tokenResult.Messages.Count != 0)
            {
               bundleResult.Messages.AddRange(tokenResult.Messages);
               var statusCode = tokenResult.Messages
                  .Where(x => x.Text.Contains("StatusCode:"))
                  .Select(x => x.Text.Replace("StatusCode:", ""))
                  .Where(x => !string.IsNullOrEmpty(x))
                  .Select(x => int.Parse(x))
                  .FirstOrDefault();
               bundleResult.Data = (System.Net.HttpStatusCode)statusCode;
               return bundleResult;
            }
            else if (tokenResult.Data == null || string.IsNullOrEmpty(tokenResult.Data.access_token))
            { bundleResult.Data = System.Net.HttpStatusCode.InternalServerError; return bundleResult; }
             
            // APPLY
            bundleResult = await this.TokenApply(this.User.Identity.Name, isPersistent, tokenResult.Data, authManager);
            return bundleResult;

         }
         catch (Exception ex) {
            bundleResult.Messages.Add(new BundleMessage(ex.ToString(), BundleMessage.enumType.Alert));
            bundleResult.Data = System.Net.HttpStatusCode.InternalServerError;
         }
         return bundleResult;
      }

      private async Task<Bundle<System.Net.HttpStatusCode>> TokenApply(string username, bool isPersistent, viewApiToken tokenResult, IAuthenticationManager authManager)
      {
         var bundleResult = new Bundle<System.Net.HttpStatusCode>();
         try
         {

            // USER
            var userResult = await this.TokenApply_GetUser(username, tokenResult);
            if (userResult.Result == false)
            {
               bundleResult.Messages.AddRange(userResult.Messages);
               bundleResult.Data = System.Net.HttpStatusCode.NotFound;
               return bundleResult;
            }
            var accessExpiration = DateTime.Now.AddSeconds(tokenResult.expires_in);
            var refreshExpiration = DateTime.Now.AddSeconds(tokenResult.client_expires_in);

            // CLAIMS
            var userClaims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, userResult.Data.ID),
               new Claim(ClaimTypes.Name, userResult.Data.UserName),
               new Claim(ClaimTypes.Email, userResult.Data.Email),
               new Claim(ClaimTypes.GivenName, userResult.Data.FullName),
               new Claim(ClaimTypes.IsPersistent, (isPersistent?1:0).ToString()),
               new Claim(ClaimTypes.AuthenticationMethod, tokenResult.token_type),
               new Claim(ClaimTypes.Authentication, tokenResult.access_token),
               new Claim("RefreshToken", tokenResult.refresh_token),
               new Claim("RefreshExpiration", refreshExpiration.ToString("yyyy-MM-dd HH:mm")),
               new Claim("SignatureExpiration", tokenResult.SignatureExpiration),
               new Claim("Roles", tokenResult.Roles),
               new Claim(ClaimTypes.Expiration, accessExpiration.ToString("yyyy-MM-dd HH:mm"))
            };

            // THIS ISNT WORKING, SO WE ARE FIXING IT TO TRUE
            isPersistent = true;

            // APPLY AUTHENTICATION
            var userIdentity = new ClaimsIdentity(userClaims, "ApplicationCookie", ClaimTypes.Name, ClaimTypes.Role);
            var authProperties = new AuthenticationProperties
            {                
               IsPersistent = isPersistent,
               IssuedUtc = DateTime.Now,
               ExpiresUtc = refreshExpiration
            };
            authManager.SignOut("ApplicationCookie");
            // authManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(userIdentity, authProperties);
            authManager.SignIn(authProperties, userIdentity);

            // USERNAME COOKIE [JUST FOR REMEMBERING ON LOGIN FORM]
            this.Response.Cookies["UserName"].Value = username;
            this.Response.Cookies["UserName"].Expires = DateTime.Now.AddYears(1);

            // RESULT
            bundleResult.Data = System.Net.HttpStatusCode.OK;
            bundleResult.Result = true;

         }
         catch (Exception ex) {
            bundleResult.Messages.Add(new BundleMessage(ex.ToString(), BundleMessage.enumType.Alert));
            bundleResult.Data = System.Net.HttpStatusCode.InternalServerError;
         }
         return bundleResult;
      }

      private async Task<Bundle<FriendCash.Auth.Model.viewUser>> TokenApply_GetUser(string username, viewApiToken tokenResult)
      {
         try
         {
            using (var api = Helper.nAPI<FriendCash.Auth.Model.viewUser>.Load(this))
            {
               if (api.ApplyLanguages() == false) { return api.Result.Data; }
               api.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResult.access_token);

               var apiUrl = "api/auth/user/" + username + "/";
               var apiMSG = await api.GetAsync(apiUrl).ConfigureAwait(false);
               api.Result = await api.ResponseAsync<FriendCash.Auth.Model.viewUser>(apiMSG).ConfigureAwait(false);

               return api.Result.Data;
            }
         }
         catch (Exception ex) { throw ex; }
      }

      #endregion

      #region TokenLoad

      private async Task<Bundle<viewApiToken>> TokenLoad(string username, string password)
      {
         try
         {

            // PARAMETERS
            var keyValues = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("grant_type", "password")
            };

            // RESULT
            return await this.TokenLoad(keyValues);

         }
         catch (Exception ex) { return null; }
      }

      private async Task<Bundle<viewApiToken>> TokenLoad(string refreshToken)
      {
         try
         {

            // PARAMETERS
            var keyValues = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("grant_type", "refresh_token")
            };

            // RESULT
            var tokenLoad = await this.TokenLoad(keyValues);
            if (!tokenLoad.Result) {
               var serverError = "StatusCode:" + ((short)System.Net.HttpStatusCode.InternalServerError).ToString();
               var badRequest = "StatusCode:" + ((short)System.Net.HttpStatusCode.BadRequest).ToString();
               var unauthorized = "StatusCode:" + ((short)System.Net.HttpStatusCode.Unauthorized).ToString();
               if (tokenLoad.Messages.Count(x => x.Text == badRequest || x.Text == serverError) != 0)
               {
                  tokenLoad.Messages.RemoveAll(x => x.Text == serverError);
                  tokenLoad.Messages.RemoveAll(x => x.Text == badRequest);
                  tokenLoad.Messages.Add(new BundleMessage(unauthorized, BundleMessage.enumType.Information));
               }
            }
            return tokenLoad;

         }
         catch (Exception ex) { return null; }
      }

      private async Task<Bundle<viewApiToken>> TokenLoad(List<KeyValuePair<string, string>> keyValues)
      {
         var resultBundle = new Bundle<viewApiToken>();
         try
         {

            using (var api = Helper.nAPI<viewApiToken>.Load(this))
            {

               // CLIENT
               var clientID = System.Configuration.ConfigurationManager.AppSettings["api:clientID"];
               var clientSecret = System.Configuration.ConfigurationManager.AppSettings["api:clientSecret"];
               keyValues.Add(new KeyValuePair<string, string>("client_id", clientID));
               keyValues.Add(new KeyValuePair<string, string>("client_secret", clientSecret));

               // PARAMETERS
               var apiUrl = "oauth/token";
               var apiParameters = new System.Net.Http.FormUrlEncodedContent(keyValues);

               // CALL
               var apiMSG = await api.PostAsync(apiUrl, apiParameters);
               if (!apiMSG.IsSuccessStatusCode)
               {
                  resultBundle.Messages.Add(new BundleMessage("StatusCode:" + (int)apiMSG.StatusCode, BundleMessage.enumType.Information));
                  resultBundle.Messages.Add(new BundleMessage("ResponseMessage:" + Model.Base.Json.Serialize(apiMSG), BundleMessage.enumType.Information));
                  return resultBundle;
               }

               // RESULT
               api.Result = await api.ResponseAsync<viewApiToken>(apiMSG);
               resultBundle = api.Result.Data;
               return resultBundle;

            }

         }
         catch (Exception ex) { resultBundle.Messages.Add(new BundleMessage(ex.ToString(), BundleMessage.enumType.Alert)); }
         return resultBundle;
      }

      #endregion    

   }

   #region viewApiToken
   internal class viewApiToken
   {
      public string access_token { get; set; }
      public string token_type { get; set; }
      public int expires_in { get; set; }
      public string refresh_token { get; set; }
      public long client_expires_in { get; set; }
      public string SignatureExpiration { get; set; }
      public string Roles { get; set; }
   }
   #endregion

}