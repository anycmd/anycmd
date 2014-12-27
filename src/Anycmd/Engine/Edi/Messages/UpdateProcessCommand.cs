
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateProcessCommand : UpdateEntityCommand<IProcessUpdateIo>, ISysCommand
    {
        public UpdateProcessCommand(IProcessUpdateIo input)
            : base(input)
        {

        }
    }
}
