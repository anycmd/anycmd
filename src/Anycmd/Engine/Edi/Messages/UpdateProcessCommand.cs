
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class UpdateProcessCommand : UpdateEntityCommand<IProcessUpdateIo>, IAnycmdCommand
    {
        public UpdateProcessCommand(IAcSession acSession, IProcessUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
