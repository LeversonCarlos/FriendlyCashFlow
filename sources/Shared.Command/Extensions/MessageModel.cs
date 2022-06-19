namespace Lewio.CashFlow.Shared;

public static class MessageModelExtensions
{
   public static MessageModel[] ToMessageModel(this Exception value)
   {
      var result = new List<MessageModel>();
      result.Add(MessageModel.CreateError(value.Message));
      if (value.InnerException != null)
         result.AddRange(ToMessageModel(value.InnerException));
      return result.ToArray();
   }
}
