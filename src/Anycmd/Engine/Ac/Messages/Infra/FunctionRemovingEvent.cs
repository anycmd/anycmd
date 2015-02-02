
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class FunctionRemovingEvent: DomainEvent
    {
        public FunctionRemovingEvent(IAcSession acSession, FunctionBase source)
            : base(acSession, source)
        {
        }
    }
}
