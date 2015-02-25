
namespace Anycmd.Engine.Ac.Functions
{
    using Functions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class FunctionRemovedEvent : DomainEvent
    {
        public FunctionRemovedEvent(IAcSession acSession, FunctionBase source)
            : base(acSession, source)
        {
        }
    }
}
