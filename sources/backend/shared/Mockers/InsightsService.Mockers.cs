using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Elesse.Shared.Tests
{
   internal class InsightsServiceMocker
   {

      readonly Mock<IInsightsService> _Mock;
      public InsightsServiceMocker() => _Mock = new Mock<IInsightsService>();
      public static InsightsServiceMocker Create() => new InsightsServiceMocker();

      public IInsightsService Build() => _Mock.Object;
   }
}
