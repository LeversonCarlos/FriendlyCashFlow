namespace FriendlyCashFlow.API.Patterns
{

   public class PatternVM
   {
      public long PatternID { get; set; }
      public Categories.enCategoryType Type { get; set; }
      public long CategoryID { get; set; }
      public Categories.CategoryVM CategoryRow { get; set; }
      public string Text { get; set; }
      public short Count { get; set; }

      internal static PatternVM Convert(PatternData value)
      {

         var result = new PatternVM
         {
            PatternID = value.PatternID,
            Type = (Categories.enCategoryType)value.Type,
            Text = value.Text,
            CategoryID = value.CategoryID,
            CategoryRow = Categories.CategoryVM.Convert(value.CategoryDetails),
            Count = value.Count
         };

         return result;
      }


   }

}
