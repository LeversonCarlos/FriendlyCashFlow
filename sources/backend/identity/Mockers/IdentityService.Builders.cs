namespace Elesse.Identity
{
   partial class IdentityService
   {

      internal static IdentityService Create() =>
         new IdentityService(null, null, null);

      internal static IdentityService Create(IdentitySettings settings) =>
         new IdentityService(settings, null, null);

      // Shared.Tests.InsightsServiceMocker.Create().Build()
      // IdentitySettings settings, IUserRepository userRepository, ITokenRepository tokenRepository

   }
}
