namespace Elesse.Categories
{

   public enum enCategoryType : short { None = 0, Expense = 1, Income = 2 }

   public interface ICategoryEntity
   {
      Shared.EntityID CategoryID { get; }
      string Text { get; }
      enCategoryType Type { get; }
      Shared.EntityID? ParentID { get; }
      string HierarchyText { get; }
   }

}
