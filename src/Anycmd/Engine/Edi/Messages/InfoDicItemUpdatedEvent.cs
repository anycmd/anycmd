
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class InfoDicItemUpdatedEvent : DomainEvent
    {
        public InfoDicItemUpdatedEvent(IAcSession userSession, InfoDicItemBase source, IInfoDicItemUpdateIo output)
            : base(userSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IInfoDicItemUpdateIo Output { get; private set; }
    }
}
