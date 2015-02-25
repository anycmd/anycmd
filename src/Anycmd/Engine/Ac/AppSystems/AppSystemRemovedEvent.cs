
namespace Anycmd.Engine.Ac.AppSystems
{
    using Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class AppSystemRemovedEvent : DomainEvent
    {
        public AppSystemRemovedEvent(IAcSession acSession, AppSystemBase source)
            : base(acSession, source)
        {
        }
    }
}
