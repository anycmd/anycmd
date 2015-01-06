
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddArchiveCommand : AddEntityCommand<IArchiveCreateIo>, IAnycmdCommand
    {
        public AddArchiveCommand(IArchiveCreateIo input)
            : base(input)
        {

        }
    }
}
