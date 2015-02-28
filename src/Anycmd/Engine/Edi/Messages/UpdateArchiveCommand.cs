
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public class UpdateArchiveCommand : UpdateEntityCommand<IArchiveUpdateIo>, IAnycmdCommand
    {
        public UpdateArchiveCommand(IAcSession acSession, IArchiveUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
