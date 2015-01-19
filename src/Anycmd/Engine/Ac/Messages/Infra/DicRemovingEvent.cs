
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class DicRemovingEvent: DomainEvent
    {
        public DicRemovingEvent(IUserSession userSession, DicBase source)
            : base(userSession, source)
        {
        }
    }
}