namespace Lewio.CashFlow.Services;

public partial class SharedResponseModel : IDisposable
{

   public bool OK { get; set; }
   public Message[]? Messages { get; set; }

   public virtual void Dispose()
   {
      Messages = null;
   }

}

partial class SharedResponseModel
{

   public partial struct Message
   {
      public string Text { get; set; }

      public enum MessageType : short { Message = 0, Warning = 1, Error = 2 }
      public MessageType Type { get; set; }
   }

   partial struct Message
   {
      public static Message Create(string text, MessageType type) =>
         new Message
         {
            Text = text,
            Type = type
         };
      public static Message CreateMessage(string text) => Create(text, MessageType.Message);
      public static Message CreateWarning(string text) => Create(text, MessageType.Warning);
      public static Message CreateError(string text) => Create(text, MessageType.Error);
   }

}
