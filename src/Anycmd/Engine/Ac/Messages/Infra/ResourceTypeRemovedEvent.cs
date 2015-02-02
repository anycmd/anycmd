
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class ResourceTypeRemovedEvent : DomainEvent
    {
        public ResourceTypeRemovedEvent(IAcSession acSession, ResourceTypeBase source)
            : base(acSession, source)
        {
        }
    }
}