
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class PositionRemovedEvent : DomainEvent
    {
        public PositionRemovedEvent(IUserSession userSession, GroupBase source)
            : base(userSession, source)
        {
        }
    }
}