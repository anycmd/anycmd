
namespace Anycmd.Engine.Ac.Dsd
{
    using Messages;

    public class UpdateDsdSetCommand : UpdateEntityCommand<IDsdSetUpdateIo>, IAnycmdCommand
    {
        public UpdateDsdSetCommand(IAcSession acSession, IDsdSetUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
