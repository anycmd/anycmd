
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Events;
    using Host.Ac.Identity;

    /// <summary>
    /// 
    /// </summary>
    public class DeveloperRemovedEvent : DomainEvent
    {
        public DeveloperRemovedEvent(IUserSession userSession, DeveloperId source) : base(userSession, source) { }
    }
}
