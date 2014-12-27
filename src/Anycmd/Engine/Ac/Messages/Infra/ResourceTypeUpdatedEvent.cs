
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class ResourceTypeUpdatedEvent : DomainEvent
    {
        public ResourceTypeUpdatedEvent(ResourceTypeBase source, IResourceTypeUpdateIo input)
            : base(source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        public IResourceTypeUpdateIo Input { get; private set; }
    }
}