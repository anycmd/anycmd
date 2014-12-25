
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateSsdSetCommand: UpdateEntityCommand<ISsdSetUpdateIo>, ISysCommand
    {
        public UpdateSsdSetCommand(ISsdSetUpdateIo input)
            : base(input)
        {

        }
    }
}
