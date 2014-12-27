
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;

    public class AddResourceCommand : AddEntityCommand<IResourceTypeCreateIo>, IAnycmdCommand
    {
        public AddResourceCommand(IResourceTypeCreateIo input)
            : base(input)
        {

        }
    }
}
