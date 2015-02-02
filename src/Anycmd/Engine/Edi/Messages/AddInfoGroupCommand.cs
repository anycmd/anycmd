
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddInfoGroupCommand : AddEntityCommand<IInfoGroupCreateIo>, IAnycmdCommand
    {
        public AddInfoGroupCommand(IAcSession userSession, IInfoGroupCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
