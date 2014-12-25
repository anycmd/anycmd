
namespace Anycmd.Engine.Host.Ac.Infra.Messages
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
