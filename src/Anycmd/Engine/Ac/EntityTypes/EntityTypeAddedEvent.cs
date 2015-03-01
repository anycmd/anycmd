
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Messages;
    
    public sealed class EntityTypeAddedEvent : EntityAddedEvent<IEntityTypeCreateIo>
    {
        public EntityTypeAddedEvent(IAcSession acSession, EntityTypeBase source, IEntityTypeCreateIo input)
            : base(acSession, source, input)
        {
        }

        internal EntityTypeAddedEvent(IAcSession acSession, EntityTypeBase source, IEntityTypeCreateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
