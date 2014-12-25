
namespace Anycmd.Engine.Host.Ac.Infra.Messages
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