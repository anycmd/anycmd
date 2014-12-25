
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class DicItemRemovedEvent : DomainEvent
    {
        public DicItemRemovedEvent(DicItemBase source)
            : base(source)
        {
        }
    }
}