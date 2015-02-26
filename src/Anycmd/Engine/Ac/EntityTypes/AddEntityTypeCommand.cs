
namespace Anycmd.Engine.Ac.EntityTypes
{


    public class AddEntityTypeCommand : AddEntityCommand<IEntityTypeCreateIo>, IAnycmdCommand
    {
        public AddEntityTypeCommand(IAcSession acSession, IEntityTypeCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
