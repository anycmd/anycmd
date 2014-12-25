
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateElementCommand : UpdateEntityCommand<IElementUpdateIo>, ISysCommand
    {
        public UpdateElementCommand(IElementUpdateIo input)
            : base(input)
        {

        }
    }
}
