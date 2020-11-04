namespace Elesse.Identity
{

   internal partial class IdentityService : IIdentityService
   {

      readonly IdentitySettings _Settings;
      readonly IUserRepository _UserRepository;
      readonly ITokenRepository _TokenRepository;

      public IdentityService(IdentitySettings settings, IUserRepository userRepository, ITokenRepository tokenRepository)
      {
         _Settings = settings;
         _UserRepository = userRepository;
         _TokenRepository = tokenRepository;
      }

   }

   public partial interface IIdentityService { }

   internal partial struct WARNINGS { }

}
