
namespace Anycmd.Engine.Ac.Messages
{
    using InOuts;

    public class UpdatePrivilegeBigramCommand : UpdateEntityCommand<IPrivilegeBigramUpdateIo>, IAnycmdCommand
    {
        public UpdatePrivilegeBigramCommand(IPrivilegeBigramUpdateIo input)
            : base(input)
        {

        }
    }
}
