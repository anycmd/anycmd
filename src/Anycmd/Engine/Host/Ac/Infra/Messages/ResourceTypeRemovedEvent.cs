
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
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