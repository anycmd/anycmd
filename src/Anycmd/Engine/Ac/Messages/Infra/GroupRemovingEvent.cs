
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class GroupRemovingEvent: DomainEvent
    {
        public GroupRemovingEvent(IAcSession acSession, GroupBase source)
            : base(acSession, source)
        {
        }
    }
}