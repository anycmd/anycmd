
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateElementCommand : UpdateEntityCommand<IElementUpdateIo>, IAnycmdCommand
    {
        public UpdateElementCommand(IElementUpdateIo input)
            : base(input)
        {

        }
    }
}
