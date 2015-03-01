
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class AddArchiveCommand : AddEntityCommand<IArchiveCreateIo>, IAnycmdCommand
    {
        public AddArchiveCommand(IAcSession acSession, IArchiveCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
