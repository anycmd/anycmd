
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using InOuts;
    using Model;

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
