
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using InOuts;

    public class PositionAddedEvent : EntityAddedEvent<IPositionCreateIo>
    {
        public PositionAddedEvent(IUserSession userSession, GroupBase source, IPositionCreateIo output)
            : base(userSession, source, output)
        {
        }
    }
}
