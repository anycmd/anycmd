
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddDicCommand : AddEntityCommand<IDicCreateIo>, IAnycmdCommand
    {
        public AddDicCommand(IUserSession userSession, IDicCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
