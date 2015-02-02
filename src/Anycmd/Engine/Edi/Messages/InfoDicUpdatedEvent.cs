
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class InfoDicUpdatedEvent : DomainEvent
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

        public IInfoDicUpdateIo Output { get; private set; }
    }
}
