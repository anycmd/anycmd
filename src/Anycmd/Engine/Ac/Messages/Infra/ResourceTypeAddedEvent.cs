
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
        public ResourceTypeAddedEvent(IUserSession userSession, ResourceTypeBase source, IResourceTypeCreateIo input)
            : base(userSession, source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
        }

        public IResourceTypeCreateIo Input { get; private set; }
    }
}