
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class UpdateTopicCommand : UpdateEntityCommand<ITopicUpdateIo>, IAnycmdCommand
    {
        public UpdateTopicCommand(IAcSession acSession, ITopicUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
