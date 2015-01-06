
namespace Anycmd.Engine.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using InOuts;

    public class GroupAddedEvent : EntityAddedEvent<IGroupCreateIo>
    {
        public GroupAddedEvent(GroupBase source, IGroupCreateIo output)
            : base(source, output)
        {
        }
    }
}
