
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class PropertyUpdatedEvent : DomainEvent
    {
        public PropertyUpdatedEvent(PropertyBase source, IPropertyUpdateIo input)
            : base(source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        public IPropertyUpdateIo Input { get; private set; }
    }
}
