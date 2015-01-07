
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ActionRemovedEvent : DomainEvent
    {
        public ActionRemovedEvent(ActionBase source) : base(source) { }
    }
}
