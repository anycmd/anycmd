
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ElementRemovedEvent : DomainEvent
    {
        public ElementRemovedEvent(ElementBase source) : base(source) { }
    }
}
