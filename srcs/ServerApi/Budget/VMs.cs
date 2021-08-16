using System;

namespace FriendlyCashFlow.API.Budget
{

   public class BudgetVM
   {
      public long BudgetID { get; set; }
      public Patterns.PatternVM PatternRow { get; set; }
      public Categories.CategoryVM CategoryRow { get; set; }
      public decimal Value { get; set; }

      internal static BudgetVM Convert(BudgetData value, Patterns.PatternData patternData)
      {
         var result = new BudgetVM
         {
            BudgetID = value.BudgetID,
            PatternRow = Patterns.PatternVM.Convert(patternData),
            Value = value.Value
         };
         return result;
      }


   }

   public class BudgetFlowVM
   {
      public Categories.CategoryVM CategoryRow { get; set; }
      public BudgetVM[] BudgetList { get; set; }
      public decimal Value { get; set; }
   }

}
