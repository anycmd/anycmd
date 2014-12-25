
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddArchiveCommand : AddEntityCommand<IArchiveCreateIo>, ISysCommand
    {
        public AddArchiveCommand(IArchiveCreateIo input)
            : base(input)
        {

        }
    }
}
