
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class DicRemovedEvent : DomainEvent
    {
        public DicRemovedEvent(DicBase source)
            : base(source)
        {
        }
    }
}