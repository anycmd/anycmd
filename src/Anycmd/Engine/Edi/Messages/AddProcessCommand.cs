
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddProcessCommand : AddEntityCommand<IProcessCreateIo>, IAnycmdCommand
    {
        public AddProcessCommand(IProcessCreateIo input)
            : base(input)
        {

        }
    }
}
