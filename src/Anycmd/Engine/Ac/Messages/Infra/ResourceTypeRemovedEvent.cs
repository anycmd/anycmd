
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class ResourceTypeRemovedEvent : DomainEvent
    {
        public ResourceTypeRemovedEvent(ResourceTypeBase source)
            : base(source)
        {
        }
    }
}