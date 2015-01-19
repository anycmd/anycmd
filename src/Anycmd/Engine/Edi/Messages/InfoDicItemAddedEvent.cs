
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
        public InfoDicItemAddedEvent(IUserSession userSession, InfoDicItemBase source, IInfoDicItemCreateIo output)
            : base(userSession, source)
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
