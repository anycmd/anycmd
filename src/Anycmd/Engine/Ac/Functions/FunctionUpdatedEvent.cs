
namespace Anycmd.Engine.Ac.Functions
{
    using Events;

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
