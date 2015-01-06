
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class AddResourceCommand : AddEntityCommand<IResourceTypeCreateIo>, IAnycmdCommand
    {
        public AddResourceCommand(IResourceTypeCreateIo input)
            : base(input)
        {

        }
    }
}
