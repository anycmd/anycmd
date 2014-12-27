
namespace Anycmd.Engine.Ac.Messages.Infra
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
