using System;

namespace Elesse.Shared
{
   public class BusinessRuleException : Exception
   {

      public BusinessRuleException(IBusinessRule brokenRule) : base(brokenRule.Message)
      {
         BrokenRule = brokenRule;
         this.Details = brokenRule.Message;
      }

      public IBusinessRule BrokenRule { get; }
      public string Details { get; }

      public override string ToString()
      {
         return $"{BrokenRule.GetType().FullName}: {BrokenRule.Message}";
      }

   }

}
