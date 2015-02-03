
namespace Anycmd.Tests
{
    using Engine.Ac;
    using Engine.Host;

    public static class AcDomainExtension
    {
        public static IAcSession GetAcSession(this IAcDomain acDomain)
        {
            var storage = acDomain.GetRequiredService<IAcSessionStorage>();
            var user = storage.GetData(acDomain.Config.CurrentAcSessionCacheKey) as IAcSession;

            return user ?? AcSessionState.Empty;
        }
    }
}
