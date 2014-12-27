
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;


    public class AddDicCommand : AddEntityCommand<IDicCreateIo>, IAnycmdCommand
    {
        public AddDicCommand(IDicCreateIo input)
            : base(input)
        {

        }
    }
}
