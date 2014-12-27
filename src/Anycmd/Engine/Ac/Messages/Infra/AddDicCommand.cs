
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;


    public class AddDicCommand : AddEntityCommand<IDicCreateIo>, ISysCommand
    {
        public AddDicCommand(IDicCreateIo input)
            : base(input)
        {

        }
    }
}
