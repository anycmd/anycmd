
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ElementAddedEvent : DomainEvent
    {
        public ElementAddedEvent(IUserSession userSession, ElementBase source) : base(userSession, source) { }
    }
}
