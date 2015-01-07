
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class AppSystemRemovingEvent: DomainEvent
    {
        public AppSystemRemovingEvent(AppSystemBase source)
            : base(source)
        {
        }
    }
}
