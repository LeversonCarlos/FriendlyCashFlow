namespace Lewio.CashFlow.Services;

public class DummyService : SharedService<DummyRequestModel, DummyResponseModel>
{

   public DummyService(IServiceProvider serviceProvider) : base(serviceProvider) { }

   protected override async Task OnExecuting()
   {
      await Task.CompletedTask;

      // SetWarningAndReturn("This", "Is", "Just", "a", "Test");
      // return;

      _Response.Date = DateTime.Now;

      SetSuccessAndReturn();
   }

}

public class DummyRequestModel : SharedRequestModel
{
   [System.ComponentModel.DataAnnotations.Required]
   public string? Test { get; set; }
}
public class DummyResponseModel : SharedResponseModel
{
   public DateTime Date { get; set; }
}
