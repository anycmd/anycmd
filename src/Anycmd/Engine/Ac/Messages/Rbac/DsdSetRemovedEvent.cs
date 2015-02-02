
namespace Anycmd.Engine.Ac.Messages.Rbac
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