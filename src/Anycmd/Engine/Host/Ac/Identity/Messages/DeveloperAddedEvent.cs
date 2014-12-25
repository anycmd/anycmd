
namespace Anycmd.Engine.Host.Ac.Identity.Messages
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class DeveloperAddedEvent : DomainEvent
    {
        public DeveloperAddedEvent(DeveloperId source) : base(source) { }
    }
}
