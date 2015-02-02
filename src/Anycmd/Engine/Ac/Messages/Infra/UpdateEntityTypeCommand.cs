
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class UpdateEntityTypeCommand : UpdateEntityCommand<IEntityTypeUpdateIo>, IAnycmdCommand
    {
        public UpdateEntityTypeCommand(IAcSession userSession, IEntityTypeUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
