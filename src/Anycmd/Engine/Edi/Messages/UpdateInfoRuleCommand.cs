
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class UpdateInfoRuleCommand : UpdateEntityCommand<IInfoRuleUpdateIo>, IAnycmdCommand
    {
        public UpdateInfoRuleCommand(IAcSession acSession, IInfoRuleUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
