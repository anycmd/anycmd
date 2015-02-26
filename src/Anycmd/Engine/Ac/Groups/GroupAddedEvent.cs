
namespace Anycmd.Engine.Ac.Groups
{

    public class GroupAddedEvent : EntityAddedEvent<IGroupCreateIo>
    {
        public GroupAddedEvent(IAcSession acSession, GroupBase source, IGroupCreateIo output)
            : base(acSession, source, output)
        {
        }
    }
}
