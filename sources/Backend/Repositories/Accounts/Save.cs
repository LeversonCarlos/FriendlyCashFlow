namespace Lewio.CashFlow.Accounts;

partial interface IAccountRepository
{
   Task Save(AccountEntity entity);
}

partial class AccountRepository
{

   public async Task Save(AccountEntity entity)
   {
      await _DataContext
         .SaveChangesAsync();
   }

}
