
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ElementActionRemovedEvent : DomainEvent
    {
        public ElementActionRemovedEvent(IAcSession acSession, ElementAction source)
            : base(acSession, source)
        {
        }
    }
}
