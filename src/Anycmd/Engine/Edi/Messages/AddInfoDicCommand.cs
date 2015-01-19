
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddInfoDicCommand : AddEntityCommand<IInfoDicCreateIo>, IAnycmdCommand
    {
        public AddInfoDicCommand(IUserSession userSession, IInfoDicCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
