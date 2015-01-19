
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using InOuts;

    public class AddDsdSetCommand : AddEntityCommand<IDsdSetCreateIo>, IAnycmdCommand
    {
        public AddDsdSetCommand(IUserSession userSession, IDsdSetCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
