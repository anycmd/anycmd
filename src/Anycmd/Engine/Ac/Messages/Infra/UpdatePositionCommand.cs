
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdatePositionCommand : UpdateEntityCommand<IPositionUpdateIo>, IAnycmdCommand
    {
        public UpdatePositionCommand(IAcSession userSession, IPositionUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
