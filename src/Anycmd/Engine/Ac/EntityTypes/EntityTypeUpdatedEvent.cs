
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class EntityTypeUpdatedEvent : DomainEvent
    {
        public EntityTypeUpdatedEvent(IAcSession acSession, EntityTypeBase source, IEntityTypeUpdateIo input)
            : base(acSession, source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        public IEntityTypeUpdateIo Input { get; private set; }
        internal bool IsPrivate { get; set; }
    }
}
