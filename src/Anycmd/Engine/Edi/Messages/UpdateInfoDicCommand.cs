
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateInfoDicCommand : UpdateEntityCommand<IInfoDicUpdateIo>, IAnycmdCommand
    {
        public UpdateInfoDicCommand(IInfoDicUpdateIo input)
            : base(input)
        {

        }
    }
}
