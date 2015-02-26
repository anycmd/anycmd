
namespace Anycmd.Engine.Ac.Ssd
{

    public class UpdateSsdSetCommand: UpdateEntityCommand<ISsdSetUpdateIo>, IAnycmdCommand
    {
        public UpdateSsdSetCommand(IAcSession acSession, ISsdSetUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
