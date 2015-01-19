
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddOrganizationCommand : AddEntityCommand<IOrganizationCreateIo>, IAnycmdCommand
    {
        public AddOrganizationCommand(IUserSession userSession, IOrganizationCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
