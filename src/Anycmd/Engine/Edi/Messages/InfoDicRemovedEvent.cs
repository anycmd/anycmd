
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class InfoDicRemovedEvent : DomainEvent
    {
        public InfoDicRemovedEvent(IAcSession acSession, InfoDicBase source) : base(acSession, source) { }

        internal InfoDicRemovedEvent(IAcSession acSession, InfoDicBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
