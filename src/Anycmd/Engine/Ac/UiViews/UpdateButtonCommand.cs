
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;


    public class UpdateButtonCommand : UpdateEntityCommand<IButtonUpdateIo>, IAnycmdCommand
    {
        public UpdateButtonCommand(IAcSession acSession, IButtonUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
