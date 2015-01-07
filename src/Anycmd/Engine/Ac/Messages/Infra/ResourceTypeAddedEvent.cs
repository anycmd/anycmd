
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class ResourceTypeAddedEvent : DomainEvent
    {
        public ResourceTypeAddedEvent(ResourceTypeBase source, IResourceTypeCreateIo input)
            : base(source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
        }

        public IResourceTypeCreateIo Input { get; private set; }
    }
}