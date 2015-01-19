
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class ResourceTypeRemovedEvent : DomainEvent
    {
        public ResourceTypeRemovedEvent(IUserSession userSession, ResourceTypeBase source)
            : base(userSession, source)
        {
        }
    }
}