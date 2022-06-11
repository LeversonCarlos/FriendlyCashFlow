namespace Friendly.CashFlow.Services;

public class DummyService : SharedService<DummyRequestModel, DummyResponseModel>
{

   public DummyService(IServiceProvider serviceProvider) : base(serviceProvider) { }

   protected override async Task OnExecuting()
   {

      _Response.Date = DateTime.Now;

      await Task.CompletedTask;
      SetSuccessAndReturn();
   }

}

public class DummyRequestModel : SharedRequestModel { }
public class DummyResponseModel : SharedResponseModel
{
   public DateTime Date { get; set; }
}
