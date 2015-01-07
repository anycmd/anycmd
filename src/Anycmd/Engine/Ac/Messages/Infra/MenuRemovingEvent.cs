
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class MenuRemovingEvent: DomainEvent
    {
        public MenuRemovingEvent(MenuBase source)
            : base(source)
        {
        }
    }
}