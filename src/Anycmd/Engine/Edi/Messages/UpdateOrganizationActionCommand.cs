
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateOrganizationActionCommand : UpdateEntityCommand<IOrganizationActionUpdateIo>, IAnycmdCommand
    {
        public UpdateOrganizationActionCommand(IUserSession userSession, IOrganizationActionUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
