
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateNodeActionCommand : UpdateEntityCommand<INodeActionUpdateIo>, IAnycmdCommand
    {
        public UpdateNodeActionCommand(INodeActionUpdateIo input)
            : base(input)
        {

        }
    }
}
