
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateTopicCommand : UpdateEntityCommand<ITopicUpdateIo>, IAnycmdCommand
    {
        public UpdateTopicCommand(ITopicUpdateIo input)
            : base(input)
        {

        }
    }
}
