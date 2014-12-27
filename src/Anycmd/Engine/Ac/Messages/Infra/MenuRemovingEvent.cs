
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    public class MenuRemovingEvent: DomainEvent
    {
        public MenuRemovingEvent(MenuBase source)
            : base(source)
        {
        }
    }
}