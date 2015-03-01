
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class UpdateStateCodeCommand : UpdateEntityCommand<IStateCodeUpdateInput>, IAnycmdCommand
    {
        public UpdateStateCodeCommand(IAcSession acSession, IStateCodeUpdateInput input)
            : base(acSession, input)
        {

        }
    }
}
