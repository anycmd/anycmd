
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Events;
    using Host.Ac.Identity;

    /// <summary>
    /// 
    /// </summary>
    public class DeveloperAddedEvent : DomainEvent
    {
        public DeveloperAddedEvent(DeveloperId source) : base(source) { }
    }
}
