
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class ResourceTypeUpdatedEvent : DomainEvent
    {
        public ResourceTypeUpdatedEvent(IAcSession userSession, ResourceTypeBase source, IResourceTypeUpdateIo input)
            : base(userSession, source)
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