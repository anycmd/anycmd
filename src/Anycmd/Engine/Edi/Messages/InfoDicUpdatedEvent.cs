
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
        public InfoDicUpdatedEvent(IUserSession userSession, InfoDicBase source, IInfoDicUpdateIo output)
            : base(userSession, source)
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
