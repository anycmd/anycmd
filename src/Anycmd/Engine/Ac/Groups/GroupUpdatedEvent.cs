
namespace Anycmd.Engine.Ac.Groups
{
    using Events;

    public sealed class GroupUpdatedEvent : DomainEvent
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
        internal bool IsPrivate { get; set; }
    }
}