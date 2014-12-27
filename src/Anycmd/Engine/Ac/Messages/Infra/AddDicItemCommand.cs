
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;


    public class AddDicItemCommand : AddEntityCommand<IDicItemCreateIo>, IAnycmdCommand
    {
        public AddDicItemCommand(IDicItemCreateIo input)
            : base(input)
        {

        }
    }
}
