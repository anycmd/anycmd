
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;


    public class AddEntityTypeCommand : AddEntityCommand<IEntityTypeCreateIo>, ISysCommand
    {
        public AddEntityTypeCommand(IEntityTypeCreateIo input)
            : base(input)
        {

        }
    }
}
