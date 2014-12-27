
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateInfoDicItemCommand : UpdateEntityCommand<IInfoDicItemUpdateIo>, ISysCommand
    {
        public UpdateInfoDicItemCommand(IInfoDicItemUpdateIo input)
            : base(input)
        {

        }
    }
}
