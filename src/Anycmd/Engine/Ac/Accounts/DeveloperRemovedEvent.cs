
namespace Anycmd.Engine.Ac.Accounts
{
    using Events;
    using Host.Ac.Identity;

    /// <summary>
    /// 
    /// </summary>
    public sealed class DeveloperRemovedEvent : DomainEvent
    {
        public DeveloperRemovedEvent(IAcSession acSession, DeveloperId source) : base(acSession, source) { }

        internal DeveloperRemovedEvent(IAcSession acSession, DeveloperId source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
