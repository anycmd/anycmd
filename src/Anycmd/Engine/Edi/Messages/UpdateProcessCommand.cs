
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateProcessCommand : UpdateEntityCommand<IProcessUpdateIo>, IAnycmdCommand
    {
        public UpdateProcessCommand(IAcSession userSession, IProcessUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
