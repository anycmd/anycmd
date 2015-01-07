
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
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