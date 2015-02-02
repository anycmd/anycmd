
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddArchiveCommand : AddEntityCommand<IArchiveCreateIo>, IAnycmdCommand
    {
        public AddArchiveCommand(IAcSession acSession, IArchiveCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
