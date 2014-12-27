
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddInfoDicCommand : AddEntityCommand<IInfoDicCreateIo>, ISysCommand
    {
        public AddInfoDicCommand(IInfoDicCreateIo input)
            : base(input)
        {

        }
    }
}
