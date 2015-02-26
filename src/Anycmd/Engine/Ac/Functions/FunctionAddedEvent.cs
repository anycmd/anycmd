
namespace Anycmd.Engine.Ac.Functions
{

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
