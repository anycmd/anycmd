
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateTopicCommand : UpdateEntityCommand<ITopicUpdateIo>, IAnycmdCommand
    {
        public UpdateTopicCommand(IAcSession userSession, ITopicUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
