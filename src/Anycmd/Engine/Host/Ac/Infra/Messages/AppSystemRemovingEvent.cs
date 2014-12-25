
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    public class AppSystemRemovingEvent: DomainEvent
    {
        public AppSystemRemovingEvent(AppSystemBase source)
            : base(source)
        {
        }
    }
}
