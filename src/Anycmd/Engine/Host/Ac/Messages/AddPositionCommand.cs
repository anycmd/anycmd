
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddPositionCommand : AddEntityCommand<IPositionCreateIo>, ISysCommand
    {
        public AddPositionCommand(IPositionCreateIo input)
            : base(input)
        {
        }
    }
}
