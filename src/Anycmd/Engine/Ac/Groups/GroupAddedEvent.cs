
namespace Anycmd.Engine.Ac.Groups
{
    using Messages;

    public class GroupAddedEvent : EntityAddedEvent<IGroupCreateIo>
    {
        public GroupAddedEvent(IAcSession acSession, GroupBase source, IGroupCreateIo output)
            : base(acSession, source, output)
        {
        }
    }
}
