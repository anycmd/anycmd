
namespace Anycmd.Engine.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using Events;
    using InOuts;

    public class PrivilegeBigramAddedEvent : DomainEvent
    {
        public PrivilegeBigramAddedEvent(PrivilegeBigramBase source, IPrivilegeBigramCreateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IPrivilegeBigramCreateIo Output { get; private set; }
    }
}
