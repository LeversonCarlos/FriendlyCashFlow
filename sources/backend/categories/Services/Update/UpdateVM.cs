namespace Elesse.Categories
{
   public class UpdateVM
   {
      public Shared.EntityID CategoryID { get; set; }
      public string Text { get; set; }
      public enCategoryType Type { get; set; }
      public Shared.EntityID ParentID { get; set; }
   }
}
