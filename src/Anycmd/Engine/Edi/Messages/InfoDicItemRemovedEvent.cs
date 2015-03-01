
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class InfoDicItemRemovedEvent : DomainEvent
    {
        public InfoDicItemRemovedEvent(IAcSession acSession, InfoDicItemBase source) : base(acSession, source) { }

        internal InfoDicItemRemovedEvent(IAcSession acSession, InfoDicItemBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
