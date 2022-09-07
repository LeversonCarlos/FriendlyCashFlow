namespace Lewio.CashFlow.Accounts;

partial interface IAccountRepository
{
   Task SaveNew(AccountEntity entity);
}

partial class AccountRepository
{

   public async Task SaveNew(AccountEntity entity)
   {
      await _DataContext
         .AddAsync(entity);
      await _DataContext
         .SaveChangesAsync();
   }

}
