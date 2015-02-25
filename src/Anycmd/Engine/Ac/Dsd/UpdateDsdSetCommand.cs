
namespace Anycmd.Engine.Ac.Dsd
{
    using InOuts;

    public class UpdateDsdSetCommand : UpdateEntityCommand<IDsdSetUpdateIo>, IAnycmdCommand
    {
        public UpdateDsdSetCommand(IAcSession acSession, IDsdSetUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
