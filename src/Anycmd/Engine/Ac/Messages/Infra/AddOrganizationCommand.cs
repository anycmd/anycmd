
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;


    public class AddOrganizationCommand : AddEntityCommand<IOrganizationCreateIo>, IAnycmdCommand
    {
        public AddOrganizationCommand(IOrganizationCreateIo input)
            : base(input)
        {

        }
    }
}
