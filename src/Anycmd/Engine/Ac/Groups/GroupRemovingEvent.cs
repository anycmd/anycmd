
namespace Anycmd.Engine.Ac.Groups
{
    using Events;

    public sealed class GroupRemovingEvent : DomainEvent
    {
        public GroupRemovingEvent(IAcSession acSession, GroupBase source)
            : base(acSession, source)
        {
        }
    }
}