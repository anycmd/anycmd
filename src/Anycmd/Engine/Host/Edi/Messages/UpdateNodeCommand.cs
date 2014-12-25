
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateNodeCommand : UpdateEntityCommand<INodeUpdateIo>, ISysCommand
    {
        public UpdateNodeCommand(INodeUpdateIo input)
            : base(input)
        {

        }
    }
}
