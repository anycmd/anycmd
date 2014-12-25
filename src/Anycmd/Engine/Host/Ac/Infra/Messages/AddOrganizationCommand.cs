
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Commands;
    using InOuts;
    using Model;


    public class AddOrganizationCommand : AddEntityCommand<IOrganizationCreateIo>, ISysCommand
    {
        public AddOrganizationCommand(IOrganizationCreateIo input)
            : base(input)
        {

        }
    }
}
