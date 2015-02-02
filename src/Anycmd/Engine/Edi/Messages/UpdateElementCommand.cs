
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateElementCommand : UpdateEntityCommand<IElementUpdateIo>, IAnycmdCommand
    {
        public UpdateElementCommand(IAcSession acSession, IElementUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
