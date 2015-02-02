
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class InfoGroupRemovedEvent : DomainEvent
    {
        public InfoGroupRemovedEvent(IAcSession acSession, InfoGroupBase source) : base(acSession, source) { }
    }
}
