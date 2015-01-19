
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ElementRemovedEvent : DomainEvent
    {
        public ElementRemovedEvent(IUserSession userSession, ElementBase source) : base(userSession, source) { }
    }
}
