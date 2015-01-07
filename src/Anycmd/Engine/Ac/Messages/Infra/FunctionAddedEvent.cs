
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class FunctionAddedEvent : EntityAddedEvent<IFunctionCreateIo>
    {
        public FunctionAddedEvent(FunctionBase source, IFunctionCreateIo input)
            : base(source, input)
        {
        }
    }
}
