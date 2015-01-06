
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateInfoDicItemCommand : UpdateEntityCommand<IInfoDicItemUpdateIo>, IAnycmdCommand
    {
        public UpdateInfoDicItemCommand(IInfoDicItemUpdateIo input)
            : base(input)
        {

        }
    }
}
