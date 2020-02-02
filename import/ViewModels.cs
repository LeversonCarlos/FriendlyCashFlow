using System;
using CsvHelper.Configuration.Attributes;

namespace Import
{

   public enum enEntryType { None = 0, Expense = 1, Income = 2 };
   public class Entry
   {
      public enEntryType Type { get; set; }
      public string Text { get; set; }

      public string Category { get; set; }

      public DateTime DueDate { get; set; }
      public decimal Value { get; set; }

      public bool Paid { get; set; }

      [Optional]
      [NullValuesAttribute("NULL")]
      public DateTime? PayDate { get; set; }

      public string Account { get; set; }

      [Optional]
      [NullValuesAttribute("NULL")]
      public string Recurrency { get; set; }
   }

   public class Transfer
   {
      public DateTime Date { get; set; }
      public decimal Value { get; set; }
      public string IncomeAccount { get; set; }
      public string ExpenseAccount { get; set; }
   }

}