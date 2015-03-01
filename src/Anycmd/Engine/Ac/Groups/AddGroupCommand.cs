
namespace Anycmd.Engine.Ac.Groups
{
    using Messages;

    public sealed class AddGroupCommand : AddEntityCommand<IGroupCreateIo>, IAnycmdCommand
    {
        public AddGroupCommand(IAcSession acSession, IGroupCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
