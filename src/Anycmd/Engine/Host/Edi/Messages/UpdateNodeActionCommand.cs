
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateNodeActionCommand : UpdateEntityCommand<INodeActionUpdateIo>, ISysCommand
    {
        public UpdateNodeActionCommand(INodeActionUpdateIo input)
            : base(input)
        {

        }
    }
}
