
namespace Anycmd.Engine.Ac.Messages.Infra
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
