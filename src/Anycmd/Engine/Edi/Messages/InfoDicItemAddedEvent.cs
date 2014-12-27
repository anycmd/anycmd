
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class InfoDicItemAddedEvent : DomainEvent
    {
        public InfoDicItemAddedEvent(InfoDicItemBase source, IInfoDicItemCreateIo output)
            : base(source)
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
