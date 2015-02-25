
namespace Anycmd.Engine.Ac.Groups
{
    using Groups;
    using InOuts;

    public class GroupAddedEvent : EntityAddedEvent<IGroupCreateIo>
    {
        public GroupAddedEvent(IAcSession acSession, GroupBase source, IGroupCreateIo output)
            : base(acSession, source, output)
        {
        }
    }
}
