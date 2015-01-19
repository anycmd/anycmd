
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddInfoDicItemCommand : AddEntityCommand<IInfoDicItemCreateIo>, IAnycmdCommand
    {
        public AddInfoDicItemCommand(IUserSession userSession, IInfoDicItemCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
