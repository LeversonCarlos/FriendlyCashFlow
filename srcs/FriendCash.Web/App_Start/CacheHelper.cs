#region Using
using System;
using System.Web.Mvc;
#endregion

namespace FriendCash.Web
{
   partial class PageExtensions
   {

      #region IncludeJS
      public static MvcHtmlString IncludeJS(this HtmlHelper helper, string fileName)
      {
         var context = helper.ViewContext.RequestContext.HttpContext;

         string fileVersion = GetFileVersion(context, fileName);
         string filePath = UrlHelper.GenerateContentUrl(fileName, context);

         var tag = new TagBuilder("script");
         tag.Attributes.Add("type", "text/javascript");
         tag.Attributes.Add("src", string.Format("{0}?v={1}", filePath, fileVersion));

         return MvcHtmlString.Create(tag.ToString());
      }
      #endregion

      #region IncludeCSS
      public static MvcHtmlString IncludeCSS(this HtmlHelper helper, string fileName)
      {
         var context = helper.ViewContext.RequestContext.HttpContext;

         string fileVersion = GetFileVersion(context, fileName);
         string filePath = UrlHelper.GenerateContentUrl(fileName, context);

         var tag = new TagBuilder("link");
         tag.Attributes.Add("type", "text/css");
         tag.Attributes.Add("rel", "stylesheet");
         tag.Attributes.Add("href", string.Format("{0}?v={1}", filePath, fileVersion));

         return MvcHtmlString.Create(tag.ToString());
      }
      #endregion

      #region GetFileVersion
      private static string GetFileVersion(System.Web.HttpContextBase context, string fileName)
      {

         if (context.Cache[fileName] == null)
         {
            var physicalPath = context.Server.MapPath(fileName);
            var version = new System.IO.FileInfo(physicalPath).LastWriteTime.ToString("yyyyMMddHHmmss");
            context.Cache.Add(physicalPath, version, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
            context.Cache[fileName] = version;
            return version;
         }
         else
         {
            return context.Cache[fileName] as string;
         }
      }
      #endregion

   }
}