namespace Lewio.CashFlow.Accounts;

partial class ModelExtensions
{

   public static AccountModel ToModel(this AccountEntity entity) =>
      new AccountModel
      {
         AccountID = entity.AccountID,
         Text = entity.Text,
         Type = (AccountTypeEnum)entity.Type,
         ClosingDay = entity.ClosingDay,
         DueDay = entity.DueDay,
         DueDate = entity.GetDueDate(),
         Active = entity.Active
      };

   public static AccountEntity ToEntity(this AccountModel model)
   {
      var entity = new AccountEntity
      {
         AccountID = model.AccountID,
      };
      entity.Apply(model);
      return entity;
   }

   public static AccountEntity Apply(this AccountEntity entity, AccountModel model)
   {
      entity.Text = model.Text;
      entity.Type = (short)model.Type;
      entity.ClosingDay = model.ClosingDay;
      entity.DueDay = model.DueDay;
      entity.Active = model.Active;
      return entity;
   }

}
