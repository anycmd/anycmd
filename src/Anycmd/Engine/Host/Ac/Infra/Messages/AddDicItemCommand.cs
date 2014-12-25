
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Commands;
    using InOuts;
    using Model;


    public class AddDicItemCommand : AddEntityCommand<IDicItemCreateIo>, ISysCommand
    {
        public AddDicItemCommand(IDicItemCreateIo input)
            : base(input)
        {

        }
    }
}
