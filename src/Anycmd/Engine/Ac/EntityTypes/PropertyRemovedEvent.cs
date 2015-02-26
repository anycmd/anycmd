
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class PropertyRemovedEvent : DomainEvent
    {
        public PropertyRemovedEvent(IAcSession acSession, PropertyBase source)
            : base(acSession, source)
        {
        }
    }
}
