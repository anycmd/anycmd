
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class InfoDicItemAddedEvent : DomainEvent
    {
        public InfoDicItemAddedEvent(IAcSession acSession, InfoDicItemBase source, IInfoDicItemCreateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IInfoDicItemCreateIo Output { get; private set; }
    }
}
