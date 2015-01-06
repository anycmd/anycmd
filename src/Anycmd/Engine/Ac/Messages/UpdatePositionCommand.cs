
namespace Anycmd.Engine.Ac.Messages
{
    using InOuts;

    public class UpdatePositionCommand : UpdateEntityCommand<IPositionUpdateIo>, IAnycmdCommand
    {
        public UpdatePositionCommand(IPositionUpdateIo input)
            : base(input)
        {

        }
    }
}
