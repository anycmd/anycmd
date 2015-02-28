
namespace Anycmd.Engine.Ac.AppSystems
{
    using Events;

    public sealed class AppSystemRemovingEvent : DomainEvent
    {
        public AppSystemRemovingEvent(IAcSession acSession, AppSystemBase source)
            : base(acSession, source)
        {
        }
    }
}
