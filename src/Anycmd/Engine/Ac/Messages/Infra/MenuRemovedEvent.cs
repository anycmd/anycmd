
namespace Anycmd.Engine.Ac.Messages.Infra
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