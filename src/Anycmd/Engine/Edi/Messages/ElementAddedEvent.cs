
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ElementAddedEvent : DomainEvent
    {
        public ElementAddedEvent(IAcSession acSession, ElementBase source) : base(acSession, source) { }
    }
}
