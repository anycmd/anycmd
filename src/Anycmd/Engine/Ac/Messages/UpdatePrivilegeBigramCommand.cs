
namespace Anycmd.Engine.Ac.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdatePrivilegeBigramCommand : UpdateEntityCommand<IPrivilegeBigramUpdateIo>, ISysCommand
    {
        public UpdatePrivilegeBigramCommand(IPrivilegeBigramUpdateIo input)
            : base(input)
        {

        }
    }
}
