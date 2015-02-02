
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ActionRemovedEvent : DomainEvent
    {
        public ActionRemovedEvent(IAcSession acSession, ActionBase source) : base(acSession, source) { }
    }
}
