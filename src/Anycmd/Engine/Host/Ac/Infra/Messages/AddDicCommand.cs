
namespace Anycmd.Engine.Host.Ac.Infra.Messages
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
