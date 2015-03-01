
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;


    public sealed class UpdateButtonCommand : UpdateEntityCommand<IButtonUpdateIo>, IAnycmdCommand
    {
        public UpdateButtonCommand(IAcSession acSession, IButtonUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
