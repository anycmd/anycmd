
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class OntologyAddedEvent : DomainEvent
    {
        #region Ctor
        public OntologyAddedEvent(OntologyBase source, IOntologyCreateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }
        #endregion

        public IOntologyCreateIo Output { get; private set; }
    }
}
