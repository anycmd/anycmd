
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

        internal GroupUpdatedEvent(IAcSession acSession, GroupBase source, IGroupUpdateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public IGroupUpdateIo Output { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}