
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdatePositionCommand : UpdateEntityCommand<IPositionUpdateIo>, IAnycmdCommand
    {
        public UpdatePositionCommand(IUserSession userSession, IPositionUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
