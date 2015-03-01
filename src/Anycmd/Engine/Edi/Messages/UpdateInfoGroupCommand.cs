
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class UpdateInfoGroupCommand : UpdateEntityCommand<IInfoGroupUpdateIo>, IAnycmdCommand
    {
        public UpdateInfoGroupCommand(IAcSession acSession, IInfoGroupUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
