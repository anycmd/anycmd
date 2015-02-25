
namespace Anycmd.Engine.Ac.Functions
{
    using Functions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class FunctionUpdatedEvent : DomainEvent
    {
        public FunctionUpdatedEvent(IAcSession acSession, FunctionBase source, IFunctionUpdateIo input)
            : base(acSession, source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        public IFunctionUpdateIo Input { get; private set; }
    }
}
