
namespace Anycmd.Engine.Ac.Groups
{
    using Messages;

    public sealed class GroupAddedEvent : EntityAddedEvent<IGroupCreateIo>
    {
        public GroupAddedEvent(IAcSession acSession, GroupBase source, IGroupCreateIo output)
            : base(acSession, source, output)
        {
        }

        internal bool IsPrivate { get; set; }
    }
}
