
namespace Anycmd.Tests
{
    using Engine.Ac;
    using Engine.Host;

    public static class AcDomainExtension
    {
        public static IUserSession GetUserSession(this IAcDomain acDomain)
        {
            var storage = acDomain.GetRequiredService<IUserSessionStorage>();
            var user = storage.GetData(acDomain.Config.CurrentUserSessionCacheKey) as IUserSession;
            if (user == null)
            {
                return UserSessionState.Empty;
            }
            return user;
        }
    }
}
