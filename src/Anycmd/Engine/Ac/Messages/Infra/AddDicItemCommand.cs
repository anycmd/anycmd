
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddDicItemCommand : AddEntityCommand<IDicItemCreateIo>, IAnycmdCommand
    {
        public AddDicItemCommand(IAcSession acSession, IDicItemCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
