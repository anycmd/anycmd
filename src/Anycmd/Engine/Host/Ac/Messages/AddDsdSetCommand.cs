
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddDsdSetCommand : AddEntityCommand<IDsdSetCreateIo>, ISysCommand
    {
        public AddDsdSetCommand(IDsdSetCreateIo input)
            : base(input)
        {

        }
    }
}
