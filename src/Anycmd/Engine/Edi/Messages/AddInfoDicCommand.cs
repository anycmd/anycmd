
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddInfoDicCommand : AddEntityCommand<IInfoDicCreateIo>, IAnycmdCommand
    {
        public AddInfoDicCommand(IAcSession userSession, IInfoDicCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
