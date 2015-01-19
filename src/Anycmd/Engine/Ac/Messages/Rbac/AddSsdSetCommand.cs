
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using InOuts;

    public class AddSsdSetCommand : AddEntityCommand<ISsdSetCreateIo>, IAnycmdCommand
    {
        public AddSsdSetCommand(IUserSession userSession, ISsdSetCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
