
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdateResourceCommand : UpdateEntityCommand<IResourceTypeUpdateIo>, IAnycmdCommand
    {
        public UpdateResourceCommand(IAcSession userSession, IResourceTypeUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
