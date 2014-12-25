
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddInfoDicItemCommand : AddEntityCommand<IInfoDicItemCreateIo>, ISysCommand
    {
        public AddInfoDicItemCommand(IInfoDicItemCreateIo input)
            : base(input)
        {

        }
    }
}
