
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class InfoDicAddedEvent : DomainEvent
    {
        public InfoDicAddedEvent(IAcSession userSession, InfoDicBase source, IInfoDicCreateIo output)
            : base(userSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IInfoDicCreateIo Output { get; private set; }
    }
}
