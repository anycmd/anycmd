
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ElementRemovedEvent : DomainEvent
    {
        public ElementRemovedEvent(ElementBase source) : base(source) { }
    }
}
