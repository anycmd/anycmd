
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdateGroupCommand : UpdateEntityCommand<IGroupUpdateIo>, IAnycmdCommand
    {
        public UpdateGroupCommand(IAcSession acSession, IGroupUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
