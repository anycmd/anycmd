
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Messages;

    public class UpdateEntityTypeCommand : UpdateEntityCommand<IEntityTypeUpdateIo>, IAnycmdCommand
    {
        public UpdateEntityTypeCommand(IAcSession acSession, IEntityTypeUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
