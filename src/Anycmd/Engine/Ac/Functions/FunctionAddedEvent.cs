
namespace Anycmd.Engine.Ac.Functions
{
    using Messages;

    public class FunctionAddedEvent : EntityAddedEvent<IFunctionCreateIo>
    {
        public FunctionAddedEvent(IAcSession acSession, FunctionBase source, IFunctionCreateIo input)
            : base(acSession, source, input)
        {
        }
    }
}
