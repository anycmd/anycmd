
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddOrganizationCommand : AddEntityCommand<IOrganizationCreateIo>, IAnycmdCommand
    {
        public AddOrganizationCommand(IOrganizationCreateIo input)
            : base(input)
        {

        }
    }
}
