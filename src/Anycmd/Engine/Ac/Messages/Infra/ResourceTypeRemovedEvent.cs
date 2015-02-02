
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class ResourceTypeRemovedEvent : DomainEvent
    {
        public ResourceTypeRemovedEvent(IAcSession userSession, ResourceTypeBase source)
            : base(userSession, source)
        {
        }
    }
}