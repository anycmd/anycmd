
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddDicItemCommand : AddEntityCommand<IDicItemCreateIo>, IAnycmdCommand
    {
        public AddDicItemCommand(IUserSession userSession, IDicItemCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
