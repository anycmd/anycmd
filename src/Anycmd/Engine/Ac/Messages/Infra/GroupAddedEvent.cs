
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using InOuts;

    public class GroupAddedEvent : EntityAddedEvent<IGroupCreateIo>
    {
        public GroupAddedEvent(IAcSession userSession, GroupBase source, IGroupCreateIo output)
            : base(userSession, source, output)
        {
        }
    }
}
