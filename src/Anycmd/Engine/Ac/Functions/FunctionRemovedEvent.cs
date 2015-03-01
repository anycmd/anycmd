
namespace Anycmd.Engine.Ac.Functions
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class FunctionRemovedEvent : DomainEvent
    {
        public FunctionRemovedEvent(IAcSession acSession, FunctionBase source)
            : base(acSession, source)
        {
        }

        internal bool IsPriviate { get; set; }
    }
}
