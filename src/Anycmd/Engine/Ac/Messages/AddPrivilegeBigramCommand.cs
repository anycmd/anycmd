
namespace Anycmd.Engine.Ac.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddPrivilegeBigramCommand : AddEntityCommand<IPrivilegeBigramCreateIo>, IAnycmdCommand
    {
        public AddPrivilegeBigramCommand(IPrivilegeBigramCreateIo input)
            : base(input)
        {

        }
    }
}
