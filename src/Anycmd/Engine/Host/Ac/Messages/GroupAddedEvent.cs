
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using InOuts;
    using Model;

    public class GroupAddedEvent : EntityAddedEvent<IGroupCreateIo>
    {
        public GroupAddedEvent(GroupBase source, IGroupCreateIo output)
            : base(source, output)
        {
        }
    }
}
