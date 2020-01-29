using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Import
{

   partial class ImportService
   {

      internal async Task<ActionResult<bool>> CreateAccountsAsync(ImportVM value)
      {
         try
         {
            var accountsService = this.GetService<Accounts.AccountsService>();

            // LOAD DATA
            var loadMessage = await accountsService.GetDataAsync();
            var loadResult = this.GetValue(loadMessage);
            if (loadResult == null) { return loadMessage.Result; }
            value.Accounts = loadResult;

            // GROUP ALL ACCOUNT TEXTS
            var accountTexts = new List<string>();
            if (value.Entries != null)
            { accountTexts.AddRange(value.Entries?.Select(x => x.Account).ToArray()); }
            if (value.Transfers != null)
            {
               accountTexts.AddRange(value.Transfers?.Select(x => x.IncomeAccount).ToArray());
               accountTexts.AddRange(value.Transfers?.Select(x => x.ExpenseAccount).ToArray());
            }
            accountTexts = accountTexts.GroupBy(x => x).Select(x => x.Key).ToList();

            // CHECK FOR NEW ACCOUNTS
            accountTexts.RemoveAll(x => value.Accounts.Select(a => a.Text).Contains(x));
            foreach (var accountText in accountTexts)
            {
               var createParam = new Accounts.AccountVM
               {
                  Text = accountText,
                  Type = Accounts.enAccountType.General,
                  Active = true
               };
               var createMessage = await accountsService.CreateAsync(createParam);
               var createResult = this.GetValue(createMessage);
               if (createResult == null) { return createMessage.Result; }
               value.Accounts.Add(createResult);
            }

            // GET ACCOUNT FUNCTION
            var getAccountID = new Func<string, long?>(accountText => value.Accounts
               .Where(a => a.Text == accountText)
               .Select(a => a.AccountID)
               .FirstOrDefault());

            // MARK ACCOUNTS ON ENTRIES
            if (value.Entries != null)
            { value.Entries.ForEach(x => x.AccountID = getAccountID(x.Account)); }

            // MARK ACCOUNTS ON TRANSFERS
            if (value.Transfers != null)
            {
               value.Transfers.ForEach(x =>
               {
                  x.IncomeAccountID = getAccountID(x.IncomeAccount);
                  x.ExpenseAccountID = getAccountID(x.ExpenseAccount);
               });
            }

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

}
