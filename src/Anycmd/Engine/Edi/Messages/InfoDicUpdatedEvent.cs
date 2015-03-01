
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public sealed class InfoDicUpdatedEvent : DomainEvent
    {
        public InfoDicUpdatedEvent(IAcSession acSession, InfoDicBase source, IInfoDicUpdateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        internal InfoDicUpdatedEvent(IAcSession acSession, InfoDicBase source, IInfoDicUpdateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public IInfoDicUpdateIo Output { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}
