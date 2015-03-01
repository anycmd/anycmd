
namespace Anycmd.Engine.Ac.Ssd
{
    using Messages;

    public sealed class UpdateSsdSetCommand : UpdateEntityCommand<ISsdSetUpdateIo>, IAnycmdCommand
    {
        public UpdateSsdSetCommand(IAcSession acSession, ISsdSetUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
