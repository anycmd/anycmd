
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateArchiveCommand : UpdateEntityCommand<IArchiveUpdateIo>, ISysCommand
    {
        public UpdateArchiveCommand(IArchiveUpdateIo input)
            : base(input)
        {

        }
    }
}
