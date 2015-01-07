
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ActionAddedEvent : DomainEvent
    {
        public ActionAddedEvent(ActionBase source) : base(source) { }
    }
}
