
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

        internal bool IsPrivate { get; set; }
    }
}
