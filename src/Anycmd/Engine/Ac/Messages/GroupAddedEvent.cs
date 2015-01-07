
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using InOuts;

    public class GroupAddedEvent : EntityAddedEvent<IGroupCreateIo>
    {
        public GroupAddedEvent(GroupBase source, IGroupCreateIo output)
            : base(source, output)
        {
        }
    }
}
