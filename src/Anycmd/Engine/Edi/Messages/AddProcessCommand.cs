
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddProcessCommand : AddEntityCommand<IProcessCreateIo>, IAnycmdCommand
    {
        public AddProcessCommand(IAcSession userSession, IProcessCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
