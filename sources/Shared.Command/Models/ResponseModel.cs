namespace Lewio.CashFlow.Shared;
#nullable disable

public partial class SharedResponseModel : IDisposable
{

   public bool OK { get; set; }
   public MessageModel[] Messages { get; set; }

   public virtual void Dispose()
   {
      Messages = null;
   }

}
