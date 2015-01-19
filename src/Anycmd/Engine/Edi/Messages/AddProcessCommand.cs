
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddProcessCommand : AddEntityCommand<IProcessCreateIo>, IAnycmdCommand
    {
        public AddProcessCommand(IUserSession userSession, IProcessCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
