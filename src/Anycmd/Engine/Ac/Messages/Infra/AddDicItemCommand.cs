
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddDicItemCommand : AddEntityCommand<IDicItemCreateIo>, IAnycmdCommand
    {
        public AddDicItemCommand(IDicItemCreateIo input)
            : base(input)
        {

        }
    }
}
