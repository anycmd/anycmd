
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateElementCommand : UpdateEntityCommand<IElementUpdateIo>, IAnycmdCommand
    {
        public UpdateElementCommand(IAcSession userSession, IElementUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
