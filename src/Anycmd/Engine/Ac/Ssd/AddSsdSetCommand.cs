
namespace Anycmd.Engine.Ac.Ssd
{
    using Messages;

    public class AddSsdSetCommand : AddEntityCommand<ISsdSetCreateIo>, IAnycmdCommand
    {
        public AddSsdSetCommand(IAcSession acSession, ISsdSetCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
