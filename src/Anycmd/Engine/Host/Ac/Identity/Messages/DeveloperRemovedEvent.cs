
namespace Anycmd.Engine.Host.Ac.Identity.Messages
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class DeveloperRemovedEvent : DomainEvent
    {
        public DeveloperRemovedEvent(DeveloperId source) : base(source) { }
    }
}
