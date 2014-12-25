
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddSsdSetCommand : AddEntityCommand<ISsdSetCreateIo>, ISysCommand
    {
        public AddSsdSetCommand(ISsdSetCreateIo input)
            : base(input)
        {

        }
    }
}
