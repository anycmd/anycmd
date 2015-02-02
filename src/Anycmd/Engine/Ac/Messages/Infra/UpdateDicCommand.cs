
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class UpdateDicCommand : UpdateEntityCommand<IDicUpdateIo>, IAnycmdCommand
    {
        public UpdateDicCommand(IAcSession acSession, IDicUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
