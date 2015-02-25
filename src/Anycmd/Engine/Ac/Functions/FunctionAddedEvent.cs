
namespace Anycmd.Engine.Ac.Functions
{
    using Functions;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class FunctionAddedEvent : EntityAddedEvent<IFunctionCreateIo>
    {
        public FunctionAddedEvent(IAcSession acSession, FunctionBase source, IFunctionCreateIo input)
            : base(acSession, source, input)
        {
        }
    }
}
