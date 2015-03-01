
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class EntityTypeRemovedEvent : DomainEvent
    {
        public EntityTypeRemovedEvent(IAcSession acSession, EntityTypeBase source)
            : base(acSession, source)
        {
        }

        internal EntityTypeRemovedEvent(IAcSession acSession, EntityTypeBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
