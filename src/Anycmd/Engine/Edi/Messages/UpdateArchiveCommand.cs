
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateArchiveCommand : UpdateEntityCommand<IArchiveUpdateIo>, IAnycmdCommand
    {
        public UpdateArchiveCommand(IAcSession acSession, IArchiveUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
