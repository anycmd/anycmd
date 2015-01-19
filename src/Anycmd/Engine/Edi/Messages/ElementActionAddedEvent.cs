
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ElementActionAddedEvent : DomainEvent
    {
        public ElementActionAddedEvent(IUserSession userSession, ElementAction source)
            : base(userSession, source)
        {
        }
    }
}
