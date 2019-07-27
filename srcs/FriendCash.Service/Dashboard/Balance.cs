#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;
#endregion

namespace FriendCash.Service.Dashboard
{
   partial class DashboardController
   {

      #region GetBalance
      [Authorize(Roles = "User,Viewer")]
      [Route("balance")]
      public async Task<IHttpActionResult> GetBalance()
      {
         try
         {

            // CURRENT DATE
            var currentDate = DateTime.Now;
            var currentYear = currentDate.Year;
            var currentMonth = currentDate.Month;
            var currentDay = currentDate.Day;

            // ACCOUNTS
            List<Model.viewBalance> accountList = null;
            using (var accountController = new Accounts.AccountController())
            {
               var accountMessage = await accountController.GetAll();
               var accountResult = (System.Web.Http.Results.OkNegotiatedContentResult<List<Accounts.Model.viewAccount>>)accountMessage;
               accountList = accountResult.Content
                  .Where(x => x.Active == true)
                  .Select(x => new Model.viewBalance()
                  {
                     idAccount = x.idAccount,
                     Text = x.Text,
                     Type = x.Type,
                     DueDay = x.DueDay,
                     ClosingDay = x.ClosingDay 
                  })
                  .ToList();
               foreach (var accountItem in accountList.Where(x => x.Type == Accounts.Model.enAccountType.CreditCard && x.DueDay.HasValue).ToList())
               {
                  try
                  {
                     accountItem.DueDate = new DateTime(currentYear, currentMonth, accountItem.DueDay.Value);
                     if (accountItem.DueDate < currentDate) { accountItem.DueDate = accountItem.DueDate.Value.AddMonths(1); }
                     accountItem.DueDateText = accountItem.DueDate.Value.ToString("yyyy-MM-dd");
                  }
                  catch { }
               }
            }

            // ENTRIES
            using (var entryController = new Entries.EntryController())
            {
               foreach (var account in accountList)
               {
                  var entryYear = currentYear;
                  var entryMonth = currentMonth;
                  var entryDate = currentDate;
                  if (account.Type == Accounts.Model.enAccountType.CreditCard && account.DueDate.HasValue)
                  {
                     entryDate = account.DueDate.Value;
                     entryYear = account.DueDate.Value.Year;
                     entryMonth = account.DueDate.Value.Month;
                  }
                  var entryMessage = await entryController.GetAll(entryYear, entryMonth, account.idAccount);
                  var entryResult = (System.Web.Http.Results.OkNegotiatedContentResult<List<Entries.Model.viewEntry>>)entryMessage;

                  // INITIAL BALANCE
                  account.InitialBalance = 0;
                  var initialBalanceModel = entryResult.Content.OrderBy(x => x.SearchDate).FirstOrDefault();
                  if (initialBalanceModel != null) { account.InitialBalance = initialBalanceModel.Balance; }

                  // CURRENT BALANCE
                  account.CurrentBalance = 0;
                  var currentBalanceModel = entryResult.Content
                     .Where(x=> x.SearchDate <= entryDate)
                     .OrderByDescending(x => x.Sorting)
                     .FirstOrDefault();
                  if (currentBalanceModel != null) { account.CurrentBalance = Math.Round(currentBalanceModel.Balance, 2); }

                  // PENDING BALANCE
                  account.PendingBalance = Math.Round(entryResult.Content
                     .Where(x => x.SearchDate <= entryDate && x.Paid == false)
                     .Select(x => x.EntryValue)
                     .Sum(), 2);
                  if (account.Type == Accounts.Model.enAccountType.CreditCard) { account.PendingBalance = 0; }
                  account.CurrentBalance -= account.PendingBalance;

               }
            }

            // GROUP
            var accountGroup = accountList
               .GroupBy(x => x.Type)
               .Select(x => new Model.viewBalanceGroup()
               {
                  Text = this.GetTranslation(string.Format("ENUM_{0}_{1}", "AccountType".ToUpper(), x.Key.ToString().ToUpper())),
                  Type = x.Key,
                  CurrentBalance = x.Sum(g => g.CurrentBalance),
                  PendingBalance = x.Sum(g => g.PendingBalance),
                  Accounts = x.Select(g => g).OrderBy(g => g.Text).ToList()
               })
               .OrderBy(x => (short)x.Type)
               .ToList();

            // RESULT
            return Ok(accountGroup);

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

   }
}
