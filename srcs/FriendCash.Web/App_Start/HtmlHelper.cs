#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
#endregion

namespace FriendCash.Web
{
   public static partial class PageExtensions
   {

      #region FsTranslationFor
      public static string FsTranslationFor<TModel>(this HtmlHelper<TModel> Html, string Value)
      {
         return ((Controllers.Base)Html.ViewContext.Controller).GetTranslationByKey(Value);
      }
      #endregion


      #region FsMetadataFor
      public static ModelMetadata FsMetadataFor<TModel, TValue>(this HtmlHelper<TModel> Html, Expression<Func<TModel, TValue>> Value)
      {
         return ModelMetadata.FromLambdaExpression<TModel, TValue>(Value, new ViewDataDictionary<TModel>(Html.ViewData));
      }

      #endregion

      #region FsDescriptionFor
      public static string FsDescriptionFor<TModel, TValue>(this HtmlHelper<TModel> Html, Expression<Func<TModel, TValue>> Value)
      {
         var sResult = FsMetadataFor(Html, Value).Description;
         if (string.IsNullOrEmpty(sResult)) { sResult = Html.DisplayNameFor(Value).ToHtmlString().ToUpper(); }
         return sResult;
      }

      #endregion


      #region FsLabelFor
      public static MvcHtmlString FsLabelFor<TModel, TValue>(this HtmlHelper<TModel> Html, Expression<Func<TModel, TValue>> Value)
      {
         var sName = Html.DisplayNameFor(Value).ToHtmlString();
         var sDisplay = Html.FsDescriptionFor(Value);
         var sValue = Html.FsTranslationFor(sDisplay);
         var sDataBind = "css:{active:" + sName + " != ''}";
         if (typeof(TValue).IsEnum == true) { sDataBind = ""; }
         return Html.Label(sValue, new { @for = sName, @data_bind = sDataBind });
       }
      #endregion

      #region FsTextBoxFor

      public static MvcHtmlString FsTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> Html, Expression<Func<TModel, TValue>> Value)
      { return FsTextBoxFor(Html, Value, string.Empty, new string[] { }); }

      public static MvcHtmlString FsTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> Html, Expression<Func<TModel, TValue>> Value, string inputClass)
      { return FsTextBoxFor(Html, Value, inputClass, new string[] { }); }

      public static MvcHtmlString FsTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> Html, Expression<Func<TModel, TValue>> Value, string[] knockoutDatabind)
      { return FsTextBoxFor(Html, Value, string.Empty, knockoutDatabind); }

      public static MvcHtmlString FsTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> Html, Expression<Func<TModel, TValue>> Value, string inputClass, string[] knockoutDatabind)
      {
         var sName = Html.DisplayNameFor(Value).ToHtmlString();
         var inputType = "text";
         // inputClass += " validate";

         // KNOCKOUT DATABIND
         var valueDatabind = string.Empty;
         foreach (var itemDatabind in knockoutDatabind) { valueDatabind += itemDatabind + ","; }

         // DATE TIME SPECIFCS
         if (typeof(TValue).IsAssignableFrom(typeof(DateTime)))
         {
            valueDatabind += "value:" + sName + "";
            inputClass += " data-mask-date apply-date-picker";
            // inputType = "date";
            return Html.TextBoxFor(Value, new { @data_bind = valueDatabind, @class = inputClass, @type = inputType, @readonly = "readonly" });
         }
         else
         {
            valueDatabind += "textInput:" + sName + "";

            // INTEGER SPECIFICS
            if (typeof(TValue).IsAssignableFrom(typeof(int)))
            { inputType = "number"; }

            // DOUBLE SPECIFICS
            if (typeof(TValue).IsAssignableFrom(typeof(double)))
            { inputClass += " data-mask-decimal"; }

            return Html.TextBoxFor(Value, new { @data_bind = valueDatabind, @class = inputClass, @type = inputType });

         }

      }

      #endregion

      #region FsCheckBoxFor
      public static MvcHtmlString FsCheckBoxFor<TModel, TValue>(this HtmlHelper<TModel> Html, Expression<Func<TModel, TValue>> Value)
      {
         var sName = Html.DisplayNameFor(Value).ToHtmlString();

         var inputHidden = Html.Hidden(sName, "false");
         var inputCheckbox = new TagBuilder("input"); 
         inputCheckbox.Attributes.Add(new KeyValuePair<string, string>("type","checkbox"));
         inputCheckbox.Attributes.Add(new KeyValuePair<string, string>("name", sName));
         inputCheckbox.Attributes.Add(new KeyValuePair<string, string>("id", sName)); 
         inputCheckbox.Attributes.Add(new KeyValuePair<string, string>("value", "true"));
         inputCheckbox.Attributes.Add(new KeyValuePair<string, string>("data-bind", "checked:" + sName));
         //Html.CheckBox(sName, true);
         var inputSpan = new TagBuilder("span"); inputSpan.AddCssClass("lever");

         var labelContainer = new TagBuilder("label");
         labelContainer.InnerHtml += inputCheckbox.ToString();
         labelContainer.InnerHtml += inputSpan.ToString();
         labelContainer.InnerHtml += inputHidden.ToHtmlString();

         var divContainer = new TagBuilder("div");
         divContainer.AddCssClass("switch");
         divContainer.InnerHtml += labelContainer.ToString();

         return MvcHtmlString.Create(divContainer.ToString());
      }
      #endregion

      #region FsDropBoxFor

      public static MvcHtmlString FsDropBoxFor<TModel, TValue>(this HtmlHelper<TModel> Html, Expression<Func<TModel, TValue>> Value)
      { return FsDropBoxFor(Html, Value, new string[] { }); }

      public static MvcHtmlString FsDropBoxFor<TModel, TValue>(this HtmlHelper<TModel> Html, Expression<Func<TModel, TValue>> Value, string[] knockoutDatabind)
      {
         var sName = Html.DisplayNameFor(Value).ToHtmlString(); // sName += "Value";

         // KNOCKOUT DATABIND
         var valueDatabind = string.Empty;
         foreach (var itemDatabind in knockoutDatabind) { valueDatabind += itemDatabind + ","; }
         valueDatabind += "value:" + sName + "";

         var oItems = FsDropBoxFor_Items(Html, typeof(TValue));
         return Html.DropDownList(sName, oItems, new { @data_bind = valueDatabind, @class = "validate" });
      }

      private static List<SelectListItem> FsDropBoxFor_Items<TModel>(this HtmlHelper<TModel> Html, Type oType)
      {
         var oData = new List<SelectListItem>();

         try
         {

            // VALIDATE
            if (oType.IsEnum == false) { return oData; }
            var sEnumName = oType.Name; 
            if (sEnumName.StartsWith("en")) { sEnumName = sEnumName.Substring(2); } 
            sEnumName = sEnumName.ToUpper();

            // CONVERT
            var oValues = (short[])Enum.GetValues(oType);
            var oTexts = (string[])Enum.GetNames(oType);
            for (int i = 0; i < oValues.Length; i++)
            {
               var iValue = oValues[i];
               var sKey = string.Format("ENUM_{0}_{1}", sEnumName, oTexts[i].ToUpper());
               oData.Add(new SelectListItem()
               {
                  Value = iValue.ToString(),
                  Text = Html.FsTranslationFor(sKey)
               });
            }

         }
         catch { throw; }

         return oData;
      }

      #endregion

      #region FsRelatedBoxFor
      public static MvcHtmlString FsRelatedBoxFor<TModel, TValue, TRelated>(this HtmlHelper<TModel> Html, Expression<Func<TModel, TValue>> Value, RelatedBox<TRelated> relatedDetails)
      {
         var sName = Html.DisplayNameFor(Value).ToHtmlString();
         // var urlHelper = new UrlHelper(Html.ViewContext.RequestContext);
         // var sUrl = urlHelper.Action("GetRelated", "Categories", new { type = "Income" });

         var paramsBuilder = new StringBuilder();
         paramsBuilder.Append("{");
         paramsBuilder.AppendFormat("'inputName': '{0}'", sName);
         paramsBuilder.Append(", ");
         paramsBuilder.AppendFormat("'inputValue': {0}", sName);
         paramsBuilder.Append(", ");
         if (relatedDetails.RelatedDataModel)
         {
            paramsBuilder.AppendFormat("'inputData': {0}RelatedData", sName);
            paramsBuilder.Append(", ");
         }
         paramsBuilder.AppendFormat("'relatedController': '{0}'", relatedDetails.Controller);
         paramsBuilder.Append(", ");
         paramsBuilder.AppendFormat("'relatedAction': '{0}'", relatedDetails.Action);
         paramsBuilder.Append(", ");
         paramsBuilder.AppendFormat("'relatedFilter': '{0}'", (relatedDetails.Filter == null ? string.Empty : Model.Base.Json.Serialize(relatedDetails.Filter)));
         paramsBuilder.Append("}");

         var inputBuilder = new StringBuilder();
         inputBuilder.Append("<!-- ko component: ");
         inputBuilder.Append("{");
         inputBuilder.AppendFormat("name:'{0}'", "related-editor");
         inputBuilder.Append(", ");
         inputBuilder.AppendFormat("params:{0}", paramsBuilder.ToString());
         inputBuilder.Append(" } -->");
         inputBuilder.Append("<!-- /ko -->");

         return MvcHtmlString.Create(inputBuilder.ToString());
      }
      #endregion

   }

   #region RelatedBox

   public class RelatedBox : RelatedBox<string>
   {
      public RelatedBox(string _Controller) : base(_Controller) { }
   }

   public class RelatedBox<T>
   {

      public RelatedBox(string _Controller) : this(_Controller, "GetRelated", default(T)) { }
      public RelatedBox(string _Controller, T _Filter) : this(_Controller, "GetRelated", _Filter) { }
      public RelatedBox(string _Controller, string _Action, T _Filter)
      { this.Controller = _Controller; this.Action = _Action; this.Filter = _Filter; }

      public string Controller { get; set; }
      public string Action { get; set; }
      public T Filter { get; set; }
      public bool RelatedDataModel { get; set; }
   }

   #endregion  

}