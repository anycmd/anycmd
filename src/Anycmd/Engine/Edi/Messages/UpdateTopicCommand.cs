
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateTopicCommand : UpdateEntityCommand<ITopicUpdateIo>, IAnycmdCommand
    {
        public UpdateTopicCommand(IAcSession acSession, ITopicUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
