
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class EntityTypeRemovedEvent : DomainEvent
    {
        public EntityTypeRemovedEvent(IAcSession acSession, EntityTypeBase source)
            : base(acSession, source)
        {
        }
    }
}
