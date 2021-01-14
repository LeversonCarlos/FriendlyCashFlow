namespace Elesse.Identity
{
   partial class IdentityService
   {

      internal static IdentityService Create() =>
         new IdentityService(null, null, null, Shared.Tests.InsightsServiceMocker.Create().Build());

      internal static IdentityService Create(IdentitySettings settings) =>
         new IdentityService(settings, null, null, Shared.Tests.InsightsServiceMocker.Create().Build());

      internal static IdentityService Create(IdentitySettings settings, IUserRepository userRepository) =>
         new IdentityService(settings, userRepository, null, Shared.Tests.InsightsServiceMocker.Create().Build());

      internal static IdentityService Create(IUserRepository userRepository, ITokenRepository tokenRepository) =>
         new IdentityService(null, userRepository, tokenRepository, Shared.Tests.InsightsServiceMocker.Create().Build());

      internal static IdentityService Create(IdentitySettings settings, IUserRepository userRepository, ITokenRepository tokenRepository) =>
         new IdentityService(settings, userRepository, tokenRepository, Shared.Tests.InsightsServiceMocker.Create().Build());

   }
}
