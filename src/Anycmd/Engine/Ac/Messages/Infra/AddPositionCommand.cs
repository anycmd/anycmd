
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class AddPositionCommand : AddEntityCommand<IPositionCreateIo>, IAnycmdCommand
    {
        public AddPositionCommand(IPositionCreateIo input)
            : base(input)
        {
        }
    }
}
