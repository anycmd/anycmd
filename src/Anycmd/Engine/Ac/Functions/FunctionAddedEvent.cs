
namespace Anycmd.Engine.Ac.Functions
{
    using Messages;

    public sealed class FunctionAddedEvent : EntityAddedEvent<IFunctionCreateIo>
    {
        public FunctionAddedEvent(IAcSession acSession, FunctionBase source, IFunctionCreateIo input)
            : base(acSession, source, input)
        {
        }

        internal bool IsPriviate { get; set; }
    }
}
