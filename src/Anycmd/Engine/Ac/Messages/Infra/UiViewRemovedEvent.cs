
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewRemovedEvent : DomainEvent
    {
        public UiViewRemovedEvent(UiViewBase source)
            : base(source)
        {
        }
    }
}
