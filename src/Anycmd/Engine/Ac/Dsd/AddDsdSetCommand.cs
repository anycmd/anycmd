
namespace Anycmd.Engine.Ac.Dsd
{
    using Messages;

    public sealed class AddDsdSetCommand : AddEntityCommand<IDsdSetCreateIo>, IAnycmdCommand
    {
        public AddDsdSetCommand(IAcSession acSession, IDsdSetCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
