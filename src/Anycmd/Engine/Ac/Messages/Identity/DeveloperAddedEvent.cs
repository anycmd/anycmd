
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Events;
    using Host.Ac.Identity;

    /// <summary>
    /// 
    /// </summary>
    public class DeveloperAddedEvent : DomainEvent
    {
        public DeveloperAddedEvent(IUserSession userSession, DeveloperId source) : base(userSession, source) { }
    }
}
