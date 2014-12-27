
namespace Anycmd.Engine.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using Events;
    using InOuts;

    public class PrivilegeBigramUpdatedEvent : DomainEvent
    {
        public PrivilegeBigramUpdatedEvent(PrivilegeBigramBase source, IPrivilegeBigramUpdateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IPrivilegeBigramUpdateIo Output { get; private set; }
    }
}
