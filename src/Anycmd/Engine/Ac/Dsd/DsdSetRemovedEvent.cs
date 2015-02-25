
namespace Anycmd.Engine.Ac.Dsd
{
    using Abstractions.Rbac;
    using Events;

    public class DsdSetRemovedEvent : DomainEvent
    {
        public DsdSetRemovedEvent(IAcSession acSession, DsdSetBase source)
            : base(acSession, source)
        {
        }
    }
}