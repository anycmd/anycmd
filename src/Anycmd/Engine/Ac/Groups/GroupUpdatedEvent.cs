
namespace Anycmd.Engine.Ac.Groups
{
    using Events;

    public class GroupUpdatedEvent : DomainEvent
    {
        public GroupUpdatedEvent(IAcSession acSession, GroupBase source, IGroupUpdateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IGroupUpdateIo Output { get; private set; }
    }
}