
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateResourceCommand : UpdateEntityCommand<IResourceTypeUpdateIo>, IAnycmdCommand
    {
        public UpdateResourceCommand(IResourceTypeUpdateIo input)
            : base(input)
        {

        }
    }
}
