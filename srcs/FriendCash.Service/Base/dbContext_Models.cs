using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FriendCash.Service.Base
{
   partial class dbContext
   {

      public DbSet<Translation.TranslationModel> Translations { get; set; }

      public DbSet<Accounts.Model.bindAccount> Accounts { get; set; }
      public DbSet<Balances.Model.dataBalance> Balances { get; set; }
      public DbSet<Categories.Model.bindCategory> Categories { get; set; }
      public DbSet<Entries.Model.bindEntry> Entries { get; set; }
      public DbSet<Entries.Model.bindPattern> Patterns { get; set; }
      public DbSet<Entries.Model.bindRecurrency> Recurrencies { get; set; }

      public DbSet<Imports.Model.bindImport> Imports { get; set; }
      public DbSet<Imports.Model.bindImportItem> ImportItems { get; set; }

   }
}