
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class PositionRemovedEvent : DomainEvent
    {
        public PositionRemovedEvent(IAcSession acSession, GroupBase source)
            : base(acSession, source)
        {
        }
    }
}