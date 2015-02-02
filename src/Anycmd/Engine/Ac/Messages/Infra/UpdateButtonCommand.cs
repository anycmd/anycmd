
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class UpdateButtonCommand : UpdateEntityCommand<IButtonUpdateIo>, IAnycmdCommand
    {
        public UpdateButtonCommand(IAcSession acSession, IButtonUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
