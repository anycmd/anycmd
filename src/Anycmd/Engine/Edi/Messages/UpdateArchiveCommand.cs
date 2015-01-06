
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateArchiveCommand : UpdateEntityCommand<IArchiveUpdateIo>, IAnycmdCommand
    {
        public UpdateArchiveCommand(IArchiveUpdateIo input)
            : base(input)
        {

        }
    }
}
