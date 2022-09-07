namespace Lewio.CashFlow.Accounts;

partial class ModelExtensions
{

   public static void EnsureValidModel(this AccountModel model)
   {

      if (model == null)
         throw new Exception("Invalid Account model");

      if (string.IsNullOrEmpty(model.Text))
         throw new Exception("Missing the Text property");
      if (model.Text.Length > 500)
         throw new Exception("The Text property must have a maximum 500 characters length");

      if (model.Type == AccountTypeEnum.CreditCard)
      {
         if (!model.DueDay.HasValue || model.DueDay <= 0 || model.DueDay >= 31)
            throw new Exception("The DueDay property for a CreditCard account must be between 1 and 30");
         if (model.ClosingDay.HasValue && (model.ClosingDay <= 0 || model.ClosingDay >= 31))
            throw new Exception("The ClosingDay property for a CreditCard account must be between 1 and 30");
      }

   }

}
