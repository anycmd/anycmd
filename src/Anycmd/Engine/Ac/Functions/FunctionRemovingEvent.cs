
namespace Anycmd.Engine.Ac.Functions
{
    using Events;

    public sealed class FunctionRemovingEvent : DomainEvent
    {
        public FunctionRemovingEvent(IAcSession acSession, FunctionBase source)
            : base(acSession, source)
        {
        }
    }
}
