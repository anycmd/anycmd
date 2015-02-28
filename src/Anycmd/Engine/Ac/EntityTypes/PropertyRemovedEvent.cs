
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

        internal bool IsPrivate { get; set; }
    }
}
