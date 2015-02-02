
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdateGroupCommand : UpdateEntityCommand<IGroupUpdateIo>, IAnycmdCommand
    {
        public UpdateGroupCommand(IAcSession userSession, IGroupUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
