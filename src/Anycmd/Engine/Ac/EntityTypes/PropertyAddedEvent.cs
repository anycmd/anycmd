
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Messages;

    public class PropertyAddedEvent : EntityAddedEvent<IPropertyCreateIo>
    {
        public PropertyAddedEvent(IAcSession acSession, PropertyBase source, IPropertyCreateIo input)
            : base(acSession, source, input)
        {
        }
    }
}
