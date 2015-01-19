
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddArchiveCommand : AddEntityCommand<IArchiveCreateIo>, IAnycmdCommand
    {
        public AddArchiveCommand(IUserSession userSession, IArchiveCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
