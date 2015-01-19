
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
        public OntologyUpdatedEvent(IUserSession userSession, OntologyBase source, IOntologyUpdateIo output)
            : base(userSession, source)
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
