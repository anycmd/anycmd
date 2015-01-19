
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class GroupRemovingEvent: DomainEvent
    {
        public GroupRemovingEvent(IUserSession userSession, GroupBase source)
            : base(userSession, source)
        {
        }
    }
}