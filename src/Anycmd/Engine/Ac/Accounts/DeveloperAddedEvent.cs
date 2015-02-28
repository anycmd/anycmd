
namespace Anycmd.Engine.Ac.Accounts
{
    using Events;
    using Host.Ac.Identity;

    /// <summary>
    /// 
    /// </summary>
    public sealed class DeveloperAddedEvent : DomainEvent
    {
        public DeveloperAddedEvent(IAcSession acSession, DeveloperId source) : base(acSession, source) { }

        internal bool IsPrivate { get; set; }
    }
}
