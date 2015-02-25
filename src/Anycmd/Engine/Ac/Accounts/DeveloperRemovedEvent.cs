
namespace Anycmd.Engine.Ac.Accounts
{
    using Events;
    using Host.Ac.Identity;

    /// <summary>
    /// 
    /// </summary>
    public class DeveloperRemovedEvent : DomainEvent
    {
        public DeveloperRemovedEvent(IAcSession acSession, DeveloperId source) : base(acSession, source) { }
    }
}
