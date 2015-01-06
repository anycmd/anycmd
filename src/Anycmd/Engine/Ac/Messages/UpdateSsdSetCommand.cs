
namespace Anycmd.Engine.Ac.Messages
{
    using InOuts;

    public class UpdateSsdSetCommand: UpdateEntityCommand<ISsdSetUpdateIo>, IAnycmdCommand
    {
        public UpdateSsdSetCommand(ISsdSetUpdateIo input)
            : base(input)
        {

        }
    }
}
