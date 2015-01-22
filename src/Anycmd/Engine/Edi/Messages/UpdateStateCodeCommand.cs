
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateStateCodeCommand : UpdateEntityCommand<IStateCodeUpdateInput>, IAnycmdCommand
    {
        public UpdateStateCodeCommand(IUserSession userSession, IStateCodeUpdateInput input)
            : base(userSession, input)
        {

        }
    }
}
