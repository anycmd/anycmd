
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class OntologyUpdatedEvent : DomainEvent
    {
        public OntologyUpdatedEvent(IAcSession acSession, OntologyBase source, IOntologyUpdateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IOntologyUpdateIo Output { get; private set; }
    }
}
