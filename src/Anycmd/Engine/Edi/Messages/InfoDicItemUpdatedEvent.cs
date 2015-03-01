
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public sealed class InfoDicItemUpdatedEvent : DomainEvent
    {
        public InfoDicItemUpdatedEvent(IAcSession acSession, InfoDicItemBase source, IInfoDicItemUpdateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        internal InfoDicItemUpdatedEvent(IAcSession acSession, InfoDicItemBase source, IInfoDicItemUpdateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public IInfoDicItemUpdateIo Output { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}
