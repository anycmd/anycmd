
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class PropertyRemovedEvent : DomainEvent
    {
        public PropertyRemovedEvent(IAcSession acSession, PropertyBase source)
            : base(acSession, source)
        {
        }

        internal PropertyRemovedEvent(IAcSession acSession, PropertyBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
