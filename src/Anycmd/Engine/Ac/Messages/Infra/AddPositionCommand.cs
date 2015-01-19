
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class AddPositionCommand : AddEntityCommand<IPositionCreateIo>, IAnycmdCommand
    {
        public AddPositionCommand(IUserSession userSession, IPositionCreateIo input)
            : base(userSession, input)
        {
        }
    }
}
