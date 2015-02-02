
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class InfoGroupAddedEvent : DomainEvent
    {
        public InfoGroupAddedEvent(IAcSession acSession, InfoGroupBase source) : base(acSession, source) { }
    }
}
