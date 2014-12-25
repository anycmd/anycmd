
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class EntityTypeRemovedEvent : DomainEvent
    {
        public EntityTypeRemovedEvent(EntityTypeBase source)
            : base(source)
        {
        }
    }
}
