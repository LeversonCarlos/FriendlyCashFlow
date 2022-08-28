namespace Lewio.Shared;

public partial class ResponseModel : IDisposable
{

   public bool OK { get; internal set; }
   public Message[]? Messages { get; internal set; }
   public ExecutionVM Execution { get; private set; } = new ExecutionVM();

   public virtual void Dispose()
   {
      Messages = null;
   }

}

partial class ResponseModel
{
   public class ExecutionVM
   {
      public DateTimeOffset Start { get; private set; } = DateTimeOffset.UtcNow;
      public DateTimeOffset? Finish { get; internal set; }
      public long DurationInMilliseconds
      {
         get
         {
            try
            {
               if (!Finish.HasValue || Finish.Value <= Start)
                  return 0;
               var durationInMilliseconds = Finish.Value.Subtract(Start).TotalMilliseconds;
               return (long)Math.Round(durationInMilliseconds, 0);
            }
            catch { return 0; }
         }
      }
   }
}
