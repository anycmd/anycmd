
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateNodeCommand : UpdateEntityCommand<INodeUpdateIo>, IAnycmdCommand
    {
        public UpdateNodeCommand(INodeUpdateIo input)
            : base(input)
        {

        }
    }
}
