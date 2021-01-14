namespace Elesse.Identity
{

   internal partial class IdentityService : IIdentityService
   {

      public IdentityService(
         IdentitySettings settings,
         IUserRepository userRepository, ITokenRepository tokenRepository,
         Shared.IInsightsService insightsService)
      {
         _Settings = settings;
         _UserRepository = userRepository;
         _TokenRepository = tokenRepository;
         _InsightsService = insightsService;
      }

      readonly IdentitySettings _Settings;
      readonly IUserRepository _UserRepository;
      readonly ITokenRepository _TokenRepository;
      readonly Shared.IInsightsService _InsightsService;

   }

   public partial interface IIdentityService { }

   internal partial struct WARNINGS { }

}
