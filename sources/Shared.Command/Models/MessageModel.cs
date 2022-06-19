namespace Lewio.CashFlow.Shared;
#nullable disable

public partial struct MessageModel
{
   public string Text { get; set; }
   public TypeEnum Type { get; set; }
}

partial struct MessageModel
{
   public enum TypeEnum : short { Message = 0, Warning = 1, Error = 2 }
}

partial struct MessageModel
{

   public static MessageModel CreateMessage(string text) =>
      Create(text, TypeEnum.Message);

   public static MessageModel CreateWarning(string text) =>
      Create(text, TypeEnum.Warning);

   public static MessageModel CreateError(string text) =>
      Create(text, TypeEnum.Error);

   internal static MessageModel Create(string text, TypeEnum type) =>
      new MessageModel
      {
         Text = text,
         Type = type
      };

}
