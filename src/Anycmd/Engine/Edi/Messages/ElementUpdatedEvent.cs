
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ElementUpdatedEvent : DomainEvent
    {
        public ElementUpdatedEvent(IAcSession acSession, ElementBase source)
            : base(acSession, source)
        {
        }
    }
}
