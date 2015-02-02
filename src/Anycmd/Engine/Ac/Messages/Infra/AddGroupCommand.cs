
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class AddGroupCommand : AddEntityCommand<IGroupCreateIo>, IAnycmdCommand
    {
        public AddGroupCommand(IAcSession userSession, IGroupCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
