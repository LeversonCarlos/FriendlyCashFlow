﻿using System;

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
      public DateTime? PayDate { get; set; }

      public string Account { get; set; }

      public string Recurrency { get; set; }
   }
}