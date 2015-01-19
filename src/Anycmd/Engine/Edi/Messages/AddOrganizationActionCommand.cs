
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddOrganizationActionCommand: AddEntityCommand<IOrganizationActionCreateIo>, IAnycmdCommand
    {
        public AddOrganizationActionCommand(IUserSession userSession, IOrganizationActionCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
