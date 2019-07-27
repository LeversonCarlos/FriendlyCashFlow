#region Using
using FriendCash.Service.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
#endregion

namespace FriendCash.Web.Helper
{

   internal class nAPI<TResult> : HttpClient
   {

      #region New
      public nAPI(Controllers.Base _Controller)
      {
         this.Result = new ApiResult<TResult>();
         this.Controller = new Func<Controllers.Base>(() => { return _Controller; });
      }
      #endregion

      #region Properties
      private Func<Controllers.Base> Controller;
      internal ApiResult<TResult> Result { get; set; }
      #endregion

      #region Load
      internal static nAPI<TResult> Load(Controllers.Base _Controller)
      {
         try
         {
            var hostAddress = ConfigurationManager.AppSettings["api:HostAddress"];
            var httpClient = new nAPI<TResult>(_Controller);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(hostAddress);
            return httpClient;
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

      #region ValidateToken
      internal async Task<bool> ValidateToken()
      {
         try
         {

            // CHECK EXPIRATION
            if (this.Controller().UserTicket.AccessExpiration > DateTime.Now.AddMinutes(1)) { return true; }

            // TOKEN REFRESH
            var refreshToken = this.Controller().UserTicket.RefreshToken;
            var isPersistent = this.Controller().UserTicket.IsPersistent;
            var refreshResult = await this.Controller().TokenApply(refreshToken, isPersistent);
            if (!refreshResult.Result || refreshResult.Data != HttpStatusCode.OK)
            { this.Result.StatusCode = refreshResult.Data; return false; }
            this.Controller().ViewBag.UserTicket = null;

            // RESULT
            return true;

         }
         catch (Exception ex)
         {
            this.Result.StatusCode = HttpStatusCode.InternalServerError;
            this.Result.Data.Messages.Add(new BundleMessage(ex.ToString(), BundleMessage.enumType.Alert));
            return false;
         }
      }
      #endregion

      #region ApplyAuthorization
      internal bool ApplyAuthorization()
      {
         try
         {

            // CHECK AUTHENTICATION
            if (this.Controller().User == null || !this.Controller().User.Identity.IsAuthenticated)
            { this.Result.StatusCode = HttpStatusCode.Forbidden; return false; }

            // PARAMETERS
            var authType = this.Controller().UserTicket.AuthorizationType;
            var authToken = this.Controller().UserTicket.Authorization;
            var authHeader = new AuthenticationHeaderValue(authType, authToken);

            // APPLY
            this.DefaultRequestHeaders.Authorization = authHeader;
            return true;

         }
         catch (Exception ex)
         {
            this.Result.StatusCode = HttpStatusCode.InternalServerError;
            this.Result.Data.Messages.Add(new BundleMessage(ex.ToString(), BundleMessage.enumType.Alert));
            return false;
         }
      }
      #endregion

      #region ApplyLanguages
      internal bool ApplyLanguages()
      {
         try
         {

            // LANGUAGES
            /* "pt-BR, pt;q=0.8, en-US;q=0.6, en;q=0.4" */
            var oLanguageList = this.Controller().Request.Headers.GetValues("Accept-Language");
            if (oLanguageList != null && oLanguageList.Length != 0)
            {
               foreach (var oLanguageListItem in oLanguageList)
               {
                  var oLanguages = oLanguageListItem.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                  foreach (var sLanguage in oLanguages)
                  {
                     var aLanguage = sLanguage.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                     var sValue = aLanguage[0].Trim(); double dQuality = 1;
                     if (aLanguage.Length > 1 && aLanguage[1].StartsWith("q="))
                     {
                        var sQuality = aLanguage[1].Substring(2);
                        double.TryParse(sQuality, out dQuality);
                     }
                     this.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(sValue));
                  }
               }
            }

            // RESULT
            return true;

         }
         catch (Exception ex)
         {
            this.Result.StatusCode = HttpStatusCode.InternalServerError;
            this.Result.Data.Messages.Add(new BundleMessage(ex.ToString(), BundleMessage.enumType.Alert));
            return false;
         }
      }
      #endregion


      #region GetAsync

      static internal async Task<ApiResult<TResult>> GetAsync(Controllers.Base _Controller, string sPath)
      { return await GetAsync(_Controller, sPath, false); }

      static internal async Task<ApiResult<TResult>> GetAsync(Controllers.Base _Controller, string sPath, bool anonymous)
      {
         using (var api = Helper.nAPI<TResult>.Load(_Controller))
         {
            if (!anonymous) {
               if (await api.ValidateToken() == false) { return api.Result; }
               if (api.ApplyAuthorization() == false) { return api.Result; }
            }
            if (api.ApplyLanguages() == false) { return api.Result; }

            var apiMSG = await api.GetAsync(sPath);
            return await api.ResponseAsync<TResult>(apiMSG);
         }
      }

      #endregion

      #region PostAsync

      static internal async Task<ApiResult<TResult>> PostAsync<TParameter>(Controllers.Base _Controller, string sPath, TParameter oData)
      { return await PostAsync(_Controller, sPath, oData, false); }

      static internal async Task<ApiResult<TResult>> PostAsync<TParameter>(Controllers.Base _Controller, string sPath, TParameter oData, bool anonymous)
      {
         using (var api = Helper.nAPI<TResult>.Load(_Controller))
         {
            if (!anonymous) {
               if (await api.ValidateToken() == false) { return api.Result; }
               if (api.ApplyAuthorization() == false) { return api.Result; }
            }
            if (api.ApplyLanguages() == false) { return api.Result; }

            var apiMSG = await api.PostAsJsonAsync(sPath, oData);
            return await api.ResponseAsync<TResult>(apiMSG);
         }
      }

      #endregion

      #region PutAsync
      static internal async Task<ApiResult<TResult>> PutAsync<TParameter>(Controllers.Base _Controller, string sPath, TParameter oData)
      {
         using (var api = Helper.nAPI<TResult>.Load(_Controller))
         {

            // AUTHENTICATION
            if (await api.ValidateToken() == false) { return api.Result; }
            if (api.ApplyAuthorization() == false) { return api.Result; }
            if (api.ApplyLanguages() == false) { return api.Result; }

            // EXECUTE
            var apiMSG = await api.PutAsJsonAsync(sPath, oData);
            return await api.ResponseAsync<TResult>(apiMSG);

         }
      }
      #endregion

      #region DeleteAsync
      static internal async Task<ApiResult<TResult>> DeleteAsync(Controllers.Base _Controller, string sPath)
      {
         using (var api = Helper.nAPI<TResult>.Load(_Controller))
         {

            // AUTHENTICATION
            if (await api.ValidateToken() == false) { return api.Result; }
            if (api.ApplyAuthorization() == false) { return api.Result; }
            if (api.ApplyLanguages() == false) { return api.Result; }

            // EXECUTE
            var apiMSG = await api.DeleteAsync(sPath);
            return await api.ResponseAsync<TResult>(apiMSG);

         }
      }
      #endregion


      #region ResponseAsync
      internal async Task<ApiResult<T>> ResponseAsync<T>(HttpResponseMessage httpMSG)
      {
         var result = new ApiResult<T>();
         try
         {
            result.StatusCode = httpMSG.StatusCode;

            if (httpMSG.IsSuccessStatusCode)
            {
               var httpJSON = await httpMSG.Content.ReadAsStringAsync();
               if (typeof(T) == typeof(string)) { result.Data.Data = (T)Convert.ChangeType(httpJSON, typeof(T)); }
               else { result.Data.Data = Helper.Json.DeserializeObject<T>(httpJSON); }
               result.Data.Result = true;
               result.OK = true;
            }

            else if (httpMSG.Content != null)
            {
               var oMSG = new BundleMessage(httpMSG.ReasonPhrase);
               if (httpMSG.StatusCode == HttpStatusCode.Forbidden) { oMSG.Type = BundleMessage.enumType.Information; }
               else if (httpMSG.StatusCode == HttpStatusCode.BadRequest) { oMSG.Type = BundleMessage.enumType.Warning; }
               if (httpMSG.Content != null) { oMSG.Text = await httpMSG.Content.ReadAsStringAsync(); }
               result.Data.Messages.Add(oMSG);
               if (result.StatusCode != HttpStatusCode.Unauthorized)
               { result.OK = true; }
            }

         }
         catch (Exception ex) {
            result.StatusCode = HttpStatusCode.InternalServerError;
            result.Data.Messages.Add(new BundleMessage(ex.Message, BundleMessage.enumType.Alert));
         }
         return result;
      }
      #endregion

   }

   #region ApiResult
   internal class ApiResult<TResult>
   {
      public ApiResult() { this.Data = new Bundle<TResult>(); }
      public bool OK { get; set; }
      public Bundle<TResult> Data { get; set; }
      public HttpStatusCode StatusCode { get; set; }
   }
   #endregion

   #region OBSOLETE
   /*
   internal class API : HttpClient
   {

      #region LoadAPI

      internal static API LoadAPI()
      {
         try
         {
            var oHttpClient = new API();
            oHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            oHttpClient.BaseAddress = new Uri(LoadAPI_HostAddress());
            return oHttpClient;
         }
         catch (Exception ex) { throw ex; }
      }

      private static string LoadAPI_HostAddress() { return ConfigurationManager.AppSettings["api:HostAddress"]; }

      #endregion

      #region LoadAsyncAPI

      internal static async Task<API> LoadAsyncAPI(Controllers.Base oController)
      {
         try
         {

            // INITIALIZE
            var oHttpClient = LoadAPI();

            // AUTHORIZATION
            if (oController.User != null && oController.User.Identity.IsAuthenticated)
            {
               var authHeader = await LoadAPI_Authorization(oController);
               if (authHeader != null) { oHttpClient.DefaultRequestHeaders.Authorization = authHeader; }
            }

            // LANGUAGES
            var oLanguages = LoadAsyncAPI_Languages(oController);
            oLanguages.ForEach(x => oHttpClient.DefaultRequestHeaders.AcceptLanguage.Add(x));

            // RESULT
            return oHttpClient;
         }
         catch (Exception ex) { throw ex; }
      }

      private static async Task<AuthenticationHeaderValue> LoadAPI_Authorization(Controllers.Base oController)
      {
         try
         {

            // TOKEN REFRESH IS NEED
            //if (oController.UserTicket.Expiration <= DateTime.Now.AddMinutes(1))
            //{
            //   var refreshToken = oController.UserTicket.RefreshToken;
            //   var refreshResult = await oController.TokenApply(refreshToken);
            //   if (refreshResult != null && !refreshResult.Result) { return null; }
            //   // if (refreshToken == oController.UserTicket.RefreshToken) { var t = ""; }
            //}

            // RESULT
            var sAuthorizationType = oController.UserTicket.AuthorizationType;
            var sAuthorization = oController.UserTicket.Authorization;
            return new AuthenticationHeaderValue(sAuthorizationType, sAuthorization);

         }
         catch (Exception ex) { throw ex; }
      }

      private static List<System.Net.Http.Headers.StringWithQualityHeaderValue> LoadAsyncAPI_Languages(Controllers.Base oController)
      { 
         // "pt-BR, pt;q=0.8, en-US;q=0.6, en;q=0.4"
         var oResult = new List<System.Net.Http.Headers.StringWithQualityHeaderValue>();
         try
         {
            var oLanguageList = oController.Request.Headers.GetValues("Accept-Language");
            if (oLanguageList != null && oLanguageList.Length != 0)
            {
               foreach (var oLanguageListItem in oLanguageList)
               {
                  var oLanguages = oLanguageListItem.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                  foreach (var sLanguage in oLanguages)
                  {
                     var aLanguage = sLanguage.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                     var sValue = aLanguage[0].Trim(); double dQuality = 1;
                     if (aLanguage.Length > 1 && aLanguage[1].StartsWith("q="))
                     {
                        var sQuality = aLanguage[1].Substring(2);
                        double.TryParse(sQuality, out dQuality);
                     }
                     oResult.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue(sValue));
                  }
               }
            }
         }
         catch (Exception ex) { throw ex; }
         return oResult;
      }

      #endregion


      #region GetAsync
      internal async Task<Bundle<TResult>> GetApiAsync<TResult>(string sPath)
      {
         var httpMSG = await this.GetAsync(sPath).ConfigureAwait(false);         
         var httpDATA = await this.GetResponseAsync<TResult>(httpMSG).ConfigureAwait(false);
         return httpDATA;
      }
      #endregion

      #region PostAsync
      internal async Task<Bundle<TResult>> PostApiAsync<TResult, TParameter>(string sPath, TParameter oData)
      {
         var httpMSG = await this.PostAsJsonAsync<TParameter>(sPath, oData);
         var httpDATA = await this.GetResponseAsync<TResult>(httpMSG);
         return httpDATA;
      }
      #endregion

      #region PutAsync
      internal async Task<Bundle<TResult>> PutApiAsync<TResult, TParameter>(string sPath, TParameter oData)
      {
         var httpMSG = await this.PutAsJsonAsync<TParameter>(sPath, oData);
         var httpDATA = await this.GetResponseAsync<TResult>(httpMSG);
         return httpDATA;
      }
      #endregion

      #region DeleteAsync
      internal async Task<Bundle<TResult>> DeleteApiAsync<TResult>(string sPath)
      {
         var httpMSG = await this.DeleteAsync(sPath);
         var httpDATA = await this.GetResponseAsync<TResult>(httpMSG);
         return httpDATA;
      }
      #endregion

      #region GetResponseAsync
      private async Task<Bundle<T>> GetResponseAsync<T>(HttpResponseMessage httpMSG)
      {
         var oBundle = new Bundle<T>();
         try
         {
            if (httpMSG.IsSuccessStatusCode)
            {
               var httpJSON = await httpMSG.Content.ReadAsStringAsync();
               if (typeof(T) == typeof(string)) { oBundle.Data = (T)Convert.ChangeType(httpJSON, typeof(T)); }
               else { oBundle.Data = Helper.Json.DeserializeObject<T>(httpJSON); }
               oBundle.Result = true;
            }
            else if (httpMSG.Content != null)
            {
               var oMSG = new BundleMessage(httpMSG.ReasonPhrase);
               if (httpMSG.StatusCode == HttpStatusCode.Forbidden) { oMSG.Type = BundleMessage.enumType.Information; }
               else if (httpMSG.StatusCode == HttpStatusCode.BadRequest) { oMSG.Type = BundleMessage.enumType.Warning; }
               if (httpMSG.Content != null) { oMSG.Text = await httpMSG.Content.ReadAsStringAsync(); }
               oBundle.Messages.Add(oMSG);
            }

         }
         catch (Exception ex) { oBundle.Messages.Add(new BundleMessage(ex.Message, BundleMessage.enumType.Alert)); }
         return oBundle;
      }
      #endregion

   }
   */
   #endregion

}