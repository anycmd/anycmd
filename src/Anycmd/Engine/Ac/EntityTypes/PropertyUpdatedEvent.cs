
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class PropertyUpdatedEvent : DomainEvent
    {
        public PropertyUpdatedEvent(IAcSession acSession, PropertyBase source, IPropertyUpdateIo input)
            : base(acSession, source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        internal PropertyUpdatedEvent(IAcSession acSession, PropertyBase source, IPropertyUpdateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public IPropertyUpdateIo Input { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}
