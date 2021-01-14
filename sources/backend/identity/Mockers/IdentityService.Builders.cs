namespace Elesse.Identity
{
   partial class IdentityService
   {

      internal static IdentityService Create() =>
         new IdentityService(null, null, null, null);

      internal static IdentityService Create(IdentitySettings settings) =>
         new IdentityService(settings, null, null, null);

      internal static IdentityService Create(IdentitySettings settings, IUserRepository userRepository) =>
         new IdentityService(settings, userRepository, null, null);

      internal static IdentityService Create(IUserRepository userRepository, ITokenRepository tokenRepository) =>
         new IdentityService(null, userRepository, tokenRepository, null);

      internal static IdentityService Create(IdentitySettings settings, IUserRepository userRepository, ITokenRepository tokenRepository) =>
         new IdentityService(settings, userRepository, tokenRepository, null);

      // Shared.Tests.InsightsServiceMocker.Create().Build()
      // IdentitySettings settings, IUserRepository userRepository, ITokenRepository tokenRepository

   }
}
