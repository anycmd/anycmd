
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Messages;

    public class AddEntityTypeCommand : AddEntityCommand<IEntityTypeCreateIo>, IAnycmdCommand
    {
        public AddEntityTypeCommand(IAcSession acSession, IEntityTypeCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
