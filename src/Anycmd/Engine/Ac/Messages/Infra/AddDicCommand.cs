
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddDicCommand : AddEntityCommand<IDicCreateIo>, IAnycmdCommand
    {
        public AddDicCommand(IDicCreateIo input)
            : base(input)
        {

        }
    }
}
