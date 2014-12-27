
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class InfoDicAddedEvent : DomainEvent
    {
        public InfoDicAddedEvent(InfoDicBase source, IInfoDicCreateIo output)
            : base(source)
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
