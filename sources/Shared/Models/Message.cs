namespace Lewio.Shared;

public partial struct Message
{
   public string Text { get; private set; }
   public TypeEnum Type { get; private set; }
}

partial struct Message
{
   public enum TypeEnum : short { Message = 0, Warning = 1, Error = 2 }

   public static Message CreateMessage(string text) => Create(text, TypeEnum.Message);
   public static Message CreateWarning(string text) => Create(text, TypeEnum.Warning);
   public static Message CreateError(string text) => Create(text, TypeEnum.Error);
   public static Message Create(string text, TypeEnum type) =>
      new Message
      {
         Text = text,
         Type = type
      };

}
