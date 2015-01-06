
namespace Anycmd.Engine.Ac.Messages
{
    using InOuts;

    public class UpdateDsdSetCommand : UpdateEntityCommand<IDsdSetUpdateIo>, IAnycmdCommand
    {
        public UpdateDsdSetCommand(IDsdSetUpdateIo input)
            : base(input)
        {

        }
    }
}
