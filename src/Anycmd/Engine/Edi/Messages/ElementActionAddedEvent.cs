
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ElementActionAddedEvent : DomainEvent
    {
        public ElementActionAddedEvent(IAcSession acSession, ElementAction source)
            : base(acSession, source)
        {
        }
    }
}
