namespace Elesse.Categories.Tests
{
   public partial class CategoryServiceTests
   {

      Shared.Results Warning(params string[] messageList) =>
         Shared.Results.GetResults("categories", Shared.enResultType.Warning, messageList);

   }
}
