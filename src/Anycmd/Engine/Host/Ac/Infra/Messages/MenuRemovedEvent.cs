
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    public class MenuRemovedEvent : DomainEvent
    {
        public MenuRemovedEvent(MenuBase source)
            : base(source)
        {
        }
    }
}