
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddEntityTypeCommand : AddEntityCommand<IEntityTypeCreateIo>, IAnycmdCommand
    {
        public AddEntityTypeCommand(IEntityTypeCreateIo input)
            : base(input)
        {

        }
    }
}
