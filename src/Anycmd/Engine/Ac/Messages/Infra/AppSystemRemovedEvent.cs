
namespace Anycmd.Engine.Ac.Messages.Infra
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
