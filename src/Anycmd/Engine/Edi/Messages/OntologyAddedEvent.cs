
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class OntologyAddedEvent : DomainEvent
    {
        public OntologyAddedEvent(OntologyBase source, IOntologyCreateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IOntologyCreateIo Output { get; private set; }
    }
}
