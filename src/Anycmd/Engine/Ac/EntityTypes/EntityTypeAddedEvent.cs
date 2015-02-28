
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Messages;
    
    public class EntityTypeAddedEvent : EntityAddedEvent<IEntityTypeCreateIo>
    {
        public EntityTypeAddedEvent(IAcSession acSession, EntityTypeBase source, IEntityTypeCreateIo input)
            : base(acSession, source, input)
        {
        }
    }
}
