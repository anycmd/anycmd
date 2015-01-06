
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateProcessCommand : UpdateEntityCommand<IProcessUpdateIo>, IAnycmdCommand
    {
        public UpdateProcessCommand(IProcessUpdateIo input)
            : base(input)
        {

        }
    }
}
