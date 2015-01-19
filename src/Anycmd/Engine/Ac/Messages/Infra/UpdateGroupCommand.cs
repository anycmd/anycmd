
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdateGroupCommand : UpdateEntityCommand<IGroupUpdateIo>, IAnycmdCommand
    {
        public UpdateGroupCommand(IUserSession userSession, IGroupUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
