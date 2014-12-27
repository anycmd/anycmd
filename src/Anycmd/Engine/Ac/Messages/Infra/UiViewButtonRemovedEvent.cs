
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewButtonRemovedEvent : DomainEvent
    {
        public UiViewButtonRemovedEvent(UiViewButtonBase source)
            : base(source)
        {
        }
    }
}
