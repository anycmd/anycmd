
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdateMenuCommand : UpdateEntityCommand<IMenuUpdateIo>, IAnycmdCommand
    {
        public UpdateMenuCommand(IMenuUpdateIo input)
            : base(input)
        {

        }
    }
}
