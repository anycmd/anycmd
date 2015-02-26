
namespace Anycmd.Engine.Ac.EntityTypes
{


    public class UpdateEntityTypeCommand : UpdateEntityCommand<IEntityTypeUpdateIo>, IAnycmdCommand
    {
        public UpdateEntityTypeCommand(IAcSession acSession, IEntityTypeUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
