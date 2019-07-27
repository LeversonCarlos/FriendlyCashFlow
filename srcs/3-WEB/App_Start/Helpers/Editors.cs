using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace FriendCash.Web.Code
{
   public static class TextBoxForExtensions
   {
      private const short iColumnSizeDefault = 9;

      #region FriendTextBoxFor

      public static MvcHtmlString FriendTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> oExpression)
      { return FriendTextBoxFor<TModel, TValue>(html, oExpression, true, iColumnSizeDefault); }

      public static MvcHtmlString FriendTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> oExpression, bool bEnabled)
      { return FriendTextBoxFor<TModel, TValue>(html, oExpression, bEnabled, iColumnSizeDefault); }

      public static MvcHtmlString FriendTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> oExpression, bool bEnabled, short iColumnSize)
      {
         /*
         ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
         string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
         string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
         if (String.IsNullOrEmpty(labelText)) { return MvcHtmlString.Empty; }
         */ 

         // DIVs
         var divLabel = new TagBuilder("div");
         divLabel.AddCssClass("col-sm-" + (12 - iColumnSize));
         var divTextbox = new TagBuilder("div");
         divTextbox.AddCssClass("col-sm-" + iColumnSize);

         // LABEL
         var oLabel = html.LabelFor(oExpression, new { @class = "control-label" });
         divLabel.InnerHtml = oLabel.ToHtmlString();

         // ATTRIBUTES
         string sClass = "form-control"; var sFormat = string.Empty; 
         string sPlaceholder = System.Web.HttpUtility.HtmlDecode(html.DisplayNameFor(oExpression).ToString());
         if (typeof(TValue).IsAssignableFrom(typeof(DateTime))) { sClass += " mask-date"; sFormat = "{0:" + Masks.GetDateMask() + "}"; }
         else if (typeof(TValue).IsAssignableFrom(typeof(double))) { sClass += " mask-value"; sFormat = "{0:f2}"; }
         object attrTextbox; 
         if (bEnabled == false)
         { attrTextbox = new { @class = sClass, @placeholder = sPlaceholder, @readonly = "readonly" }; }
         else
         { attrTextbox = new { @class = sClass, @placeholder = sPlaceholder }; }
         

         // TEXTBOX 
         var oTextbox = html.TextBoxFor(oExpression, sFormat, attrTextbox);
         divTextbox.InnerHtml += oTextbox.ToHtmlString();

         // VALIDATION
         var oValidation = html.ValidationMessageFor(oExpression, string.Empty, new { @class = "help-block" });
         divTextbox.InnerHtml += oValidation.ToHtmlString(); ;

         // GROUP
         TagBuilder divGroup = new TagBuilder("div");
         divGroup.AddCssClass("form-group");
         divGroup.InnerHtml = divLabel.ToString() + divTextbox.ToString();

         var oReturn = MvcHtmlString.Create(divGroup.ToString());
         return oReturn;
       }

      #endregion

      #region FriendPasswordFor

      public static MvcHtmlString FriendPasswordFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> oExpression)
      { return FriendPasswordFor<TModel, TValue>(html, oExpression, true, iColumnSizeDefault); }

      public static MvcHtmlString FriendPasswordFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> oExpression, bool bEnabled)
      { return FriendPasswordFor<TModel, TValue>(html, oExpression, bEnabled, iColumnSizeDefault); }

      public static MvcHtmlString FriendPasswordFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> oExpression, bool bEnabled, short iColumnSize)
      {

         // DIVs
         var divLabel = new TagBuilder("div");
         divLabel.AddCssClass("col-sm-" + (12 - iColumnSize));
         var divTextbox = new TagBuilder("div");
         divTextbox.AddCssClass("col-sm-" + iColumnSize);

         // LABEL
         var oLabel = html.LabelFor(oExpression, new { @class = "control-label" });
         divLabel.InnerHtml = oLabel.ToHtmlString();

         // ATTRIBUTES
         string sPlaceholder = System.Web.HttpUtility.HtmlDecode(html.DisplayNameFor(oExpression).ToString());
         object attrTextbox;
         if (bEnabled == false)
         { attrTextbox = new { @class = "form-control", @placeholder = sPlaceholder, @readonly = "readonly" }; }
         else
         { attrTextbox = new { @class = "form-control", @placeholder = sPlaceholder }; }

         // TEXTBOX 
         var oTextbox = html.PasswordFor(oExpression, attrTextbox);
         divTextbox.InnerHtml += oTextbox.ToHtmlString();

         // VALIDATION
         var oValidation = html.ValidationMessageFor(oExpression, string.Empty, new { @class = "help-block" });
         divTextbox.InnerHtml += oValidation.ToHtmlString(); ;

         // GROUP
         TagBuilder divGroup = new TagBuilder("div");
         divGroup.AddCssClass("form-group");
         divGroup.InnerHtml = divLabel.ToString() + divTextbox.ToString();

         var oReturn = MvcHtmlString.Create(divGroup.ToString());
         return oReturn;
      }

      #endregion

      #region FriendCheckboxFor

      public static MvcHtmlString FriendCheckboxFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, bool>> oExpression)
      { return FriendCheckboxFor<TModel>(html, oExpression, true, iColumnSizeDefault); }

      public static MvcHtmlString FriendCheckboxFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, bool>> oExpression, bool bEnabled)
      { return FriendCheckboxFor<TModel>(html, oExpression, bEnabled, iColumnSizeDefault); }

      public static MvcHtmlString FriendCheckboxFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, bool>> oExpression, bool bEnabled, short iColumnSize)
      {

         // DIVs
         var divLabel = new TagBuilder("div");
         divLabel.AddCssClass("col-sm-" + (12 - iColumnSize));
         var divTextbox = new TagBuilder("div");
         divTextbox.AddCssClass("col-sm-" + iColumnSize);

         // LABEL
         var oLabel = html.LabelFor(oExpression, new { @class = "control-label" });
         divLabel.InnerHtml = oLabel.ToHtmlString();

         // VALUE
         var sValue = html.DisplayTextFor(oExpression).ToString().ToLower();

         // CHECK LABEL
         var oCheckLabel = new TagBuilder("label");
         oCheckLabel.AddCssClass("btn");
         oCheckLabel.AddCssClass("btn-default");
         if (sValue == "true") { oCheckLabel.AddCssClass("active"); }

         // CHECK BOX 
         var oCheckBox = html.CheckBoxFor(oExpression);
         oCheckLabel.InnerHtml += oCheckBox.ToHtmlString();

         // CHECK ICON
         var oCheckIcon = new TagBuilder("i");
         oCheckIcon.AddCssClass("glyphicon");
         oCheckIcon.AddCssClass("glyphicon-check");
         oCheckLabel.InnerHtml += oCheckIcon.ToString();

         // SPAN
         var oCheckSpan = new TagBuilder("span");
         oCheckSpan.Attributes["data-toggle"] = "buttons";
         oCheckSpan.Attributes["title"] = html.DisplayNameFor(oExpression).ToHtmlString();
         oCheckSpan.InnerHtml += oCheckLabel.ToString();
         divTextbox.InnerHtml += oCheckSpan.ToString();

         // VALIDATION
         var oValidation = html.ValidationMessageFor(oExpression, string.Empty, new { @class = "help-block" });
         divTextbox.InnerHtml += oValidation.ToHtmlString(); ;

         // GROUP
         TagBuilder divGroup = new TagBuilder("div");
         divGroup.AddCssClass("form-group");
         divGroup.InnerHtml += divLabel.ToString();
         divGroup.InnerHtml += divTextbox.ToString();

         var oReturn = MvcHtmlString.Create(divGroup.ToString());
         return oReturn;
      }

      #endregion

      #region FriendDropDownListFor

      public static MvcHtmlString FriendDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> oExpression, Type oEnum)
      { return FriendDropDownListFor<TModel, TValue>(html, oExpression, oEnum, true, iColumnSizeDefault); }

      public static MvcHtmlString FriendDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> oExpression, Type oEnum, bool bEnabled)
      { return FriendDropDownListFor<TModel, TValue>(html, oExpression, oEnum, bEnabled, iColumnSizeDefault); }

      public static MvcHtmlString FriendDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> oExpression, Type oEnum, bool bEnabled, short iColumnSize)
      {

         // DIVs
         var divLabel = new TagBuilder("div");
         divLabel.AddCssClass("col-sm-" + (12 - iColumnSize));
         var divDropDown = new TagBuilder("div");
         divDropDown.AddCssClass("col-sm-" + iColumnSize);

         // LABEL
         var oLabel = html.LabelFor(oExpression, new { @class = "control-label" });
         divLabel.InnerHtml = oLabel.ToHtmlString();

         // ATTRIBUTES
         string sPlaceholder = System.Web.HttpUtility.HtmlDecode(html.DisplayNameFor(oExpression).ToString());
         object attrTextbox;
         if (bEnabled == false)
         { attrTextbox = new { @class = "form-control", @placeholder = sPlaceholder, @readonly = "readonly", @disabled = "disabled" }; }
         else
         { attrTextbox = new { @class = "form-control", @placeholder = sPlaceholder }; }

         // DROPDOWN
         var oList = new SelectList(Enum.GetValues(oEnum));
         var oDropDown = html.DropDownListFor(oExpression, oList, attrTextbox);
         divDropDown.InnerHtml += oDropDown.ToHtmlString();
         if (bEnabled == false) { divDropDown.InnerHtml += html.HiddenFor(oExpression).ToHtmlString(); }

         // VALIDATION
         var oValidation = html.ValidationMessageFor(oExpression, string.Empty, new { @class = "help-block" });
         divDropDown.InnerHtml += oValidation.ToHtmlString(); ;

         // GROUP
         TagBuilder divGroup = new TagBuilder("div");
         divGroup.AddCssClass("form-group");
         divGroup.InnerHtml = divLabel.ToString() + divDropDown.ToString();

         var oReturn = MvcHtmlString.Create(divGroup.ToString());
         return oReturn;
      }

      #endregion

      #region FriendPlanningListFor

      public static MvcHtmlString FriendPlanningListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> oExpression, IEnumerable<Model.Planning> oPlannings)
      { return FriendPlanningListFor<TModel, TValue>(html, oExpression, oPlannings, true, iColumnSizeDefault); }

      public static MvcHtmlString FriendPlanningListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> oExpression, IEnumerable<Model.Planning> oPlannings, bool bEnabled)
      { return FriendPlanningListFor<TModel, TValue>(html, oExpression, oPlannings, bEnabled, iColumnSizeDefault); }

      public static MvcHtmlString FriendPlanningListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> oExpression, IEnumerable<Model.Planning> oPlannings, bool bEnabled, short iColumnSize)
      {

         // DIVs
         var divLabel = new TagBuilder("div");
         divLabel.AddCssClass("col-sm-" + (12 - iColumnSize));
         var divDropDown = new TagBuilder("div");
         divDropDown.AddCssClass("col-sm-" + iColumnSize);

         // LABEL
         var oLabel = html.LabelFor(oExpression, new { @class = "control-label" });
         divLabel.InnerHtml = oLabel.ToHtmlString();

         // ATTRIBUTES
         string sPlaceholder = System.Web.HttpUtility.HtmlDecode(html.DisplayNameFor(oExpression).ToString());
         object attrTextbox;
         if (bEnabled == false)
         { attrTextbox = new { @class = "form-control monospace", @placeholder = sPlaceholder, @readonly = "readonly", @disabled = "disabled" }; }
         else
         { attrTextbox = new { @class = "form-control monospace", @placeholder = sPlaceholder }; }

         // DROPDOWN
         var oList = FriendTreeDownListFor_GetList(oPlannings);
         var oDropDown = html.DropDownListFor(oExpression, oList, attrTextbox);
         divDropDown.InnerHtml += oDropDown.ToHtmlString();
         if (bEnabled == false) { divDropDown.InnerHtml += html.HiddenFor(oExpression).ToHtmlString(); }

         // VALIDATION
         var oValidation = html.ValidationMessageFor(oExpression, string.Empty, new { @class = "help-block" });
         divDropDown.InnerHtml += oValidation.ToHtmlString(); ;

         // GROUP
         TagBuilder divGroup = new TagBuilder("div");
         divGroup.AddCssClass("form-group");
         divGroup.InnerHtml = divLabel.ToString() + divDropDown.ToString();

         var oReturn = MvcHtmlString.Create(divGroup.ToString());
         return oReturn;
      }

      private static SelectList FriendTreeDownListFor_GetList(IEnumerable<Model.Planning> oPlannings)
      {
         var oList = new List<SelectListItem>();

         oList.Add(new SelectListItem() { Value = "0", Text = string.Empty });
         if (oPlannings != null) { FriendTreeDownListFor_GetList(ref oList, oPlannings, 0); }

         return new SelectList(oList.AsEnumerable(), "Value", "Text");
       }

      private static void FriendTreeDownListFor_GetList(ref List<SelectListItem> oList, IEnumerable<Model.Planning> oPlannings, int iLevel)
      {
         foreach (var oPlanning in oPlannings.OrderBy(DATA => DATA.Description))
         {
            if (oPlanning.RowStatus == Model.Base.enRowStatus.Active)
            {
               var sSpace = Convert.ToChar(".");
               var sTab = new string(sSpace, iLevel);
               var sText = sTab + oPlanning.Description;
               var iValue = oPlanning.idPlanning.ToString();
               oList.Add(new SelectListItem() { Value = iValue, Text = sText });

               if (oPlanning.Childs != null && oPlanning.Childs.Count() != 0)
               { FriendTreeDownListFor_GetList(ref oList, oPlanning.Childs, (iLevel + 1)); }
            }
          }
      }

      #endregion

      #region FriendAutoCompleteFor

      public static MvcHtmlString FriendAutoCompleteFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> oExpression, AutoCompleteView oViewModel)
      { return FriendAutoCompleteFor<TModel, TValue>(html, oExpression,oViewModel,  true, iColumnSizeDefault); }

      public static MvcHtmlString FriendAutoCompleteFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> oExpression, AutoCompleteView oViewModel, bool bEnabled)
      { return FriendAutoCompleteFor<TModel, TValue>(html, oExpression, oViewModel, bEnabled, iColumnSizeDefault); }

      public static MvcHtmlString FriendAutoCompleteFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> oExpression, AutoCompleteView oViewModel, bool bEnabled, short iColumnSize)
      {

         // VIEW MODEL
         oViewModel.NameForValue = html.NameFor(oExpression).ToString();

         // DIVs
         var divLabel = new TagBuilder("div");
         divLabel.AddCssClass("col-sm-" + (12 - iColumnSize));
         var divTextbox = new TagBuilder("div");
         divTextbox.AddCssClass("col-sm-" + iColumnSize);

         // LABEL
         var oLabel = html.LabelFor(oExpression, new { @class = "control-label" });
         divLabel.InnerHtml = oLabel.ToHtmlString();

         // ATTRIBUTES
         string sPlaceholder = System.Web.HttpUtility.HtmlDecode(html.DisplayNameFor(oExpression).ToString());
         object attrTextbox;
         if (bEnabled == false)
         { attrTextbox = new { @class = "form-control", @placeholder = sPlaceholder, @readonly = "readonly" }; }
         else
         { attrTextbox = new { @class = "form-control", @placeholder = sPlaceholder }; }

         // HIDDEN
         var oHidden = html.HiddenFor(oExpression);
         divTextbox.InnerHtml += oHidden.ToHtmlString();

         // TEXTBOX 
         var oTextbox = html.TextBox(oViewModel.NameForDescription, oViewModel.ContentDescription, attrTextbox);
         divTextbox.InnerHtml += oTextbox.ToHtmlString();

         // SCRIPT
         var oScript = html.Partial("AutoComplete", oViewModel);
         divTextbox.InnerHtml += oScript.ToHtmlString();

         // VALIDATION
         var oValidation = html.ValidationMessageFor(oExpression, string.Empty, new { @class = "help-block" });
         divTextbox.InnerHtml += oValidation.ToHtmlString(); 

         // GROUP
         TagBuilder divGroup = new TagBuilder("div");
         divGroup.AddCssClass("form-group");
         divGroup.InnerHtml = divLabel.ToString() + divTextbox.ToString();

         var oReturn = MvcHtmlString.Create(divGroup.ToString());
         return oReturn;
      }

      #endregion

      #region FriendCommandForCreate

      public static MvcHtmlString FriendCommandForCreate<TModel>(this HtmlHelper<TModel> html)
      { return FriendCommandForCreate(html, new CommandCreate() { Action = new UrlHelper(html.ViewContext.RequestContext).Action("New") }); }

      public static MvcHtmlString FriendCommandForCreate<TModel>(this HtmlHelper<TModel> html, CommandCreate oCreateModel)
      {

         // BUTTON
         var oButton = html.Partial("Command_Base", oCreateModel);

         return MvcHtmlString.Create(oButton.ToString());
      }

      #endregion

      #region FriendCommandForEdit

      public static MvcHtmlString FriendCommandForEdit<TModel>(this HtmlHelper<TModel> html, CommandSave oSaveModel)
      { return FriendCommandForEdit<TModel>(html, iColumnSizeDefault, oSaveModel, null); }

      public static MvcHtmlString FriendCommandForEdit<TModel>(this HtmlHelper<TModel> html, CommandSave oSaveModel, CommandRemove oRemoveModel)
      { return FriendCommandForEdit<TModel>(html, iColumnSizeDefault, oSaveModel, oRemoveModel); }

      public static MvcHtmlString FriendCommandForEdit<TModel>(this HtmlHelper<TModel> html, short iColumnSize, CommandSave oSaveModel, CommandRemove oRemoveModel)
      {

         // MAIN
         string sMain = string.Empty;

         // REMOVE
         var oRemove = FriendCommandForEdit_Remove(html, iColumnSize, oRemoveModel);
         sMain += oRemove.ToString();

         // SAVE
         var oSave = FriendCommandForEdit_Save(html, iColumnSize, oSaveModel);
         sMain += oSave.ToString();

         // RETURN
         var oReturn = MvcHtmlString.Create(sMain);
         return oReturn;
      }

      private static TagBuilder FriendCommandForEdit_GetButton(string sTag, string sText, string sIcon, string sButtonClass)
      {

         // ICON
         var oIcon = new TagBuilder("span");
         oIcon.AddCssClass("glyphicon");
         oIcon.AddCssClass("glyphicon-" + sIcon);

         // TEXT
         var oText = new TagBuilder("span");
         oText.AddCssClass("hidden-xs");
         oText.InnerHtml = " " + sText;

         // BUTTON
         var oButton = new TagBuilder(sTag);
         oButton.AddCssClass("btn");
         oButton.AddCssClass("btn-" + sButtonClass);
         oButton.InnerHtml = oIcon.ToString() + oText.ToString();

         return oButton;
       }

      private static TagBuilder FriendCommandForEdit_Save<TModel>(this HtmlHelper<TModel> html, short iColumnSize, CommandSave oModel)
      {

         // DIV
         var oDiv = new TagBuilder("div");
         oDiv.AddCssClass("col-xs-" + iColumnSize);
         oDiv.AddCssClass("text-right");
         if (oModel == null) { return oDiv; }

         // SAVE
         var oSave = html.Partial("Command_Save", oModel);
         oDiv.InnerHtml += oSave.ToString();
         
         return oDiv;
       }

      private static TagBuilder FriendCommandForEdit_Remove<TModel>(this HtmlHelper<TModel> html, short iColumnSize, CommandRemove oModel)
      {

         // DIV
         var oDiv = new TagBuilder("div");
         oDiv.AddCssClass("col-xs-" + (12 - iColumnSize));
         if (oModel == null) { return oDiv; }

         // BUTTON
         var oButton = html.Partial("Command_Remove", oModel);
         oDiv.InnerHtml += oButton.ToString();

         return oDiv;
      }

      #endregion

   }
}
