
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class UpdateButtonCommand : UpdateEntityCommand<IButtonUpdateIo>, IAnycmdCommand
    {
        public UpdateButtonCommand(IButtonUpdateIo input)
            : base(input)
        {

        }
    }
}
