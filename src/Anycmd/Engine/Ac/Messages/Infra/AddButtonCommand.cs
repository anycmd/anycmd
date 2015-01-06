
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddButtonCommand : AddEntityCommand<IButtonCreateIo>, IAnycmdCommand
    {
        public AddButtonCommand(IButtonCreateIo input)
            : base(input)
        {

        }
    }
}
