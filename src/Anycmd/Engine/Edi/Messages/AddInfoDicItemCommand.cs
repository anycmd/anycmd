
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddInfoDicItemCommand : AddEntityCommand<IInfoDicItemCreateIo>, IAnycmdCommand
    {
        public AddInfoDicItemCommand(IAcSession userSession, IInfoDicItemCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
