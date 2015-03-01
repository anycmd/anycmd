
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class UpdateElementCommand : UpdateEntityCommand<IElementUpdateIo>, IAnycmdCommand
    {
        public UpdateElementCommand(IAcSession acSession, IElementUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
