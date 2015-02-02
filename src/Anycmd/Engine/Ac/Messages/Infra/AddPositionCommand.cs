
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class AddPositionCommand : AddEntityCommand<IPositionCreateIo>, IAnycmdCommand
    {
        public AddPositionCommand(IAcSession userSession, IPositionCreateIo input)
            : base(userSession, input)
        {
        }
    }
}
