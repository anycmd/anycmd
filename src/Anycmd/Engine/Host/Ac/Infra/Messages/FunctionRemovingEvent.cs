
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    public class FunctionRemovingEvent: DomainEvent
    {
        public FunctionRemovingEvent(FunctionBase source)
            : base(source)
        {
        }
    }
}
