namespace Elesse.Identity
{

   internal partial class IdentityService : Shared.BaseService, IIdentityService
   {

      public IdentityService(
         IdentitySettings settings,
         IUserRepository userRepository, ITokenRepository tokenRepository,
         Shared.IInsightsService insightsService)
         : base("identity", insightsService)
      {
         _Settings = settings;
         _UserRepository = userRepository;
         _TokenRepository = tokenRepository;
      }

      readonly IdentitySettings _Settings;
      readonly IUserRepository _UserRepository;
      readonly ITokenRepository _TokenRepository;

   }

   public partial interface IIdentityService { }

   internal partial struct WARNINGS { }

}
