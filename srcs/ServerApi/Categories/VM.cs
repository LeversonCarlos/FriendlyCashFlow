namespace FriendlyCashFlow.API.Categories
{

   public enum enCategoryType : short { None = 0, Expense = 1, Income = 2 }

   public class CategoryTypeVM
   {
      public enCategoryType CategoryTypeID { get; set; }
      public string Text { get; set; }
   }

}
