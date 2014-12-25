
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ActionRemovedEvent : DomainEvent {
        public ActionRemovedEvent(ActionBase source) : base(source) { }
    }
}
