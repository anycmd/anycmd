
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public sealed class InfoDicAddedEvent : DomainEvent
    {
        public InfoDicAddedEvent(IAcSession acSession, InfoDicBase source, IInfoDicCreateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IInfoDicCreateIo Output { get; private set; }
        internal bool IsPrivate { get; set; }
    }
}
