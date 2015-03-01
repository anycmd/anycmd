
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Messages;

    public sealed class PropertyAddedEvent : EntityAddedEvent<IPropertyCreateIo>
    {
        public PropertyAddedEvent(IAcSession acSession, PropertyBase source, IPropertyCreateIo input)
            : base(acSession, source, input)
        {
        }

        internal PropertyAddedEvent(IAcSession acSession, PropertyBase source, IPropertyCreateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
