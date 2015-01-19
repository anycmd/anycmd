
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateArchiveCommand : UpdateEntityCommand<IArchiveUpdateIo>, IAnycmdCommand
    {
        public UpdateArchiveCommand(IUserSession userSession, IArchiveUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
