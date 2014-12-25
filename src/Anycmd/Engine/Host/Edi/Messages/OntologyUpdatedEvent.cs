
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class OntologyUpdatedEvent : DomainEvent
    {
        #region Ctor
        public OntologyUpdatedEvent(OntologyBase source, IOntologyUpdateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }
        #endregion

        public IOntologyUpdateIo Output { get; private set; }
    }
}
