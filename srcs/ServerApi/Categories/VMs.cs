namespace FriendlyCashFlow.API.Categories
{

   public enum enCategoryType : short { None = 0, Expense = 1, Income = 2 }
   public class CategoryTypeVM : Base.EnumVM<enCategoryType> { }

   public class CategoryVM
   {

      public long CategoryID { get; set; }
      public string Text { get; set; }
      public enCategoryType Type { get; set; }
      public long? ParentID { get; set; }
      public CategoryVM ParentRow { get; set; }
      public string HierarchyText { get; set; }
      public string[] SplitedText { get; set; }

      internal static CategoryVM Convert(CategoryData value, CategoryData parentRow = null)
      {

         var result = new CategoryVM
         {
            CategoryID = value.CategoryID,
            Text = value.Text,
            Type = (enCategoryType)value.Type,
            ParentID = value.ParentID,
            HierarchyText = value.HierarchyText
         };

         if (!string.IsNullOrEmpty(result.HierarchyText))
         { result.SplitedText = result.HierarchyText.Split(" / ", System.StringSplitOptions.RemoveEmptyEntries); }

         if (parentRow != null)
         { result.ParentRow = CategoryVM.Convert(parentRow); }

         return result;
      }

   }

}
