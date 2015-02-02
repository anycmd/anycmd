
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddDicItemCommand : AddEntityCommand<IDicItemCreateIo>, IAnycmdCommand
    {
        public AddDicItemCommand(IAcSession userSession, IDicItemCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
