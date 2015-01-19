
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class PropertyRemovedEvent : DomainEvent
    {
        public PropertyRemovedEvent(IUserSession userSession, PropertyBase source)
            : base(userSession, source)
        {
        }
    }
}
