
namespace Anycmd.Engine.Ac.Ssd
{

    public class AddSsdSetCommand : AddEntityCommand<ISsdSetCreateIo>, IAnycmdCommand
    {
        public AddSsdSetCommand(IAcSession acSession, ISsdSetCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
