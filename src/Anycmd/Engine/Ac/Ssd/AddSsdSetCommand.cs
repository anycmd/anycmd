
namespace Anycmd.Engine.Ac.Ssd
{
    using Messages;

    public sealed class AddSsdSetCommand : AddEntityCommand<ISsdSetCreateIo>, IAnycmdCommand
    {
        public AddSsdSetCommand(IAcSession acSession, ISsdSetCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
