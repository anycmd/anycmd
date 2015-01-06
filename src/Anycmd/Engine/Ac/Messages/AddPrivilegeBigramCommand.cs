
namespace Anycmd.Engine.Ac.Messages
{
    using InOuts;

    public class AddPrivilegeBigramCommand : AddEntityCommand<IPrivilegeBigramCreateIo>, IAnycmdCommand
    {
        public AddPrivilegeBigramCommand(IPrivilegeBigramCreateIo input)
            : base(input)
        {

        }
    }
}
