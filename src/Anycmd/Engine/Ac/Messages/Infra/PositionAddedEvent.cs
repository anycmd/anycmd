
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using InOuts;

    public class PositionAddedEvent : EntityAddedEvent<IPositionCreateIo>
    {
        public PositionAddedEvent(GroupBase source, IPositionCreateIo output)
            : base(source, output)
        {
        }
    }
}
