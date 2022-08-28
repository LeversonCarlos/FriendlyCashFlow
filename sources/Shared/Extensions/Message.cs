namespace Lewio.Shared;

public static class MessageExtensions
{

   public static Message[] ToMessages(this Exception value)
   {
      var result = new List<Message>();

      var type = Message.TypeEnum.Warning;
      if (value.Message.StartsWith("Error: "))
         type = Message.TypeEnum.Error;
      result.Add(Message.Create(value.Message, type));

      if (value.InnerException != null)
         result.AddRange(ToMessages(value.InnerException));

      return result.ToArray();
   }

   public static Exception ToException(this Message[] value)
   {
      if (value == null || value.Length == 0)
         return new Exception("Invalid empty messages list");
      var messages = value
         .Select(x => new
         {
            Prefix = (x.Type == Message.TypeEnum.Error ? "Error: " : ""),
            x.Text
         })
         .Select(x => $"{x.Prefix}{x.Text}")
         .ToArray();
      return new Exception(messages);
   }

}
